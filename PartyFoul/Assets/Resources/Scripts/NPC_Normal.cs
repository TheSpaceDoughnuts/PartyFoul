using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC_Normal : PartyPerson {
    //private
    private Vector2 lastDirection = Vector2.zero;
    private bool _hasBeer;
	private bool _hasFun;
    private float _elapsedTime;
    private float _wanderProgress;
    private float _beerElapsed;
    private float _funElapsed;
    private GameObject _beerTarget;
    private Object _beerIconRes;
    private Object _funIconRes;
    private GameObject _beerIcon;
    private GameObject _funIcon;
    private GameObject _happyFace;
    private GameObject _sadFace;
    //protected
    protected RoomController _room;
    //public
	public float waitTargetTime = 1;
    public float wanderDistance = 0.5f;
    public float beerDurationSec = 15;
    public float funDurationSec = 10;

    protected override void OnStart()
    {
        base.OnStart();

        _beerIconRes = Resources.Load("Prefabs/beer_cup_icon");
        _beerIcon = (GameObject)Instantiate(_beerIconRes);
        _beerIcon.transform.SetParent(gameObject.transform);
        _beerIcon.transform.localPosition = Vector2.zero;
        _beerIcon.SetActive(false);

        _funIconRes = Resources.Load("Prefabs/fun_icon");
        _funIcon = (GameObject)Instantiate(_funIconRes);
        _funIcon.transform.SetParent(gameObject.transform);
        _funIcon.transform.localPosition = Vector2.zero;
        _happyFace = _funIcon.transform.GetChild(0).gameObject;
        _sadFace = _funIcon.transform.GetChild(1).gameObject;
        _sadFace.SetActive(false);
        _happyFace.SetActive(false);

        Debug.Log("Target after beer: " + _beerTarget);
    }

	protected override void OnUpdate ()
	{
        base.OnUpdate();

		_elapsedTime += Time.deltaTime;
        //waited long enough and not already seeking a target
        bool waitedLongEnough = _elapsedTime > waitTargetTime;
		if(waitedLongEnough && !_hasTarget){
            if (_hasBeer)
            {
                Mingle();
            }
            else
            {
                if (!_hasFun)
                {
                    Wander();
                }
            }
            _elapsedTime = 0;
        }

		if(AttemptArrival())
		{
            //_elapsedTime = 0;
            ResetWaitTimer();
        }
        //order may be important, if we force arrival
        CalculateProgress();

        EnjoyBeer();

        EnjoyFun();
	}

    void BecomeBored()
    {
        if (_hasFun)
        {
            _hasFun = false;
            _happyFace.SetActive(false);
            //_sadFace.SetActive(true);
            GameManager.instance.UpdateFunText();
        }
    }

    void CalculateProgress()
    {
        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        Vector2 remainder = currentPosition - _target;
        float remainderLength;
        //if we reached the target the remainder will be a zero vector with infinite sqrMag
        if (remainder.sqrMagnitude == 0)
            remainderLength = 0;
        else
            remainderLength = remainder.magnitude;
        
        _wanderProgress = 1 - (remainderLength / wanderDistance);
    }

    void EnjoyFun()
    {
        if (_hasFun)
        {
            _funElapsed += Time.deltaTime;
            if (_funElapsed > funDurationSec)
            {
                BecomeBored();
            }
        }
    }

    void EnjoyBeer()
    {
        if (_hasBeer)
        {
            _beerElapsed += Time.deltaTime;
            if (_beerElapsed > beerDurationSec)
            {
                Debug.Log("Finished Beer Time!\n\n");
                RelinquishBeer();
            }
        }
    }

    void HaveFun()
    {
        if (!_hasFun)
        {
            _hasFun = true;
            _funElapsed = 0;
            _happyFace.SetActive(true);
            _sadFace.SetActive(false);
            GameManager.instance.UpdateFunText();
        }
    }

	void ForceArrive()
	{
        Vector2 currentPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
		SetTarget(currentPosition);
	}

    NPC_Normal GetScript(GameObject npc)
    {
        return npc.GetComponent<NPC_Normal>();
    }

    void Mingle()
    {
        if (_beerTarget != null)
        {
            NPC_Normal npc = GetScript(_beerTarget);
            if (!npc.HasFun)
            {
                Vector2 dir = _beerTarget.transform.position - gameObject.transform.position;
                float len = dir.magnitude;
                dir.Normalize();
                WanderTowards(dir, Mathf.Min(len, wanderDistance));
            }
        }
        else
        {
            if (_room != null)
            {
                List<GameObject> npcs = _room.GetNormalNPCs();
                int rand = Random.Range(0, npcs.Count);
                _beerTarget = npcs[rand];
            }
            else
            {
                Debug.Log("NPC not assigned to room!");
            }
        }
    }

    void OnCollisionStay2D(Collision2D info)
    {
        if (_hasTarget)
        {
            //just so we dont jump to conclusions, wait a 100 milliseconds
            if (_elapsedTime > 0.1f)
                ForceArrive();
        }
        if (info.gameObject.tag == "Beer")
        {
            RecieveBeer();
        }
        else if (info.gameObject.tag == "NPC")
        {
            if (_hasBeer)
            {
                NPC_Normal buddy = GetScript(info.gameObject);
                buddy.HaveFun();
                HaveFun();
                _beerTarget = info.gameObject;
            }
        }
    }

    void RecieveBeer()
    {
		if(_hasBeer)
			return;

        _hasBeer = true;
        HaveFun();
        _beerIcon.SetActive(true);
		GameManager.instance.AddBeer (1);
        _beerElapsed = 0;
    }

    void RelinquishBeer()
    {
        _hasBeer = false;
        //Remove Beer from global beer reservoir
        //GameManager.instance.AddBeer(-1);
        _beerIcon.SetActive(false);
        _beerElapsed = 0;
        _beerTarget = null;
    }

    void ResetWaitTimer()
    {
        //if we barely made progress we should get a headstart on waiting for the next move
        float inverseProgress = 1 - _wanderProgress;
        Debug.Log("Progress: " + _wanderProgress + ", HeadStart: " + inverseProgress);
        float dampenHeadStart = 0.5f;
        _elapsedTime = Mathf.Max(inverseProgress * waitTargetTime * dampenHeadStart, 0);
    }

    public void SetRoom(RoomController room)
    {
        _room = room;
    }

	void Wander()
	{
		Vector2 dir = Vector2.zero;
		dir = Random.insideUnitCircle;
		dir.Normalize();
		WanderTowards(dir, wanderDistance);
	}
	void WanderTowards(Vector2 direction, float distance)
	{
        Vector2 dirScaled = direction * distance;
        Vector2 target = gameObject.transform.position;
        target.x += dirScaled.x;
        target.y += dirScaled.y;
        SetTarget(target);
		lastDirection = direction;
	}

	public bool HasFun {
		get { return _hasFun; }
	}

	public bool HasBeer {
		get {return _hasBeer; }
	}
}