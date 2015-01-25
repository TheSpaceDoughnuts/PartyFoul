using UnityEngine;
using System.Collections;

public class NPC_Normal : PartyPerson {
    //private
    private Vector2 lastDirection = Vector2.zero;
    private bool _hasBeer;
	private bool _hasFun;
    private float _elapsedTime;
    private float _wanderProgress;
    private Object _beerIconRes;
    private GameObject _beerIcon;
    //public
	public float waitTargetTime = 1;
    public float wanderDistance = 0.5f;

    protected override void OnStart()
    {
        base.OnStart();

        _beerIconRes = Resources.Load("Prefabs/beer_cup_icon");
        _beerIcon = (GameObject)Instantiate(_beerIconRes);
        _beerIcon.transform.SetParent(gameObject.transform);
        _beerIcon.transform.localPosition = Vector2.zero;
        _beerIcon.SetActive(false);
    }

	protected override void OnUpdate ()
	{
        base.OnUpdate();

		_elapsedTime += Time.deltaTime;
        //waited long enough and not already seeking a target
        bool waitedLongEnough = _elapsedTime > waitTargetTime;
		if(waitedLongEnough && !_hasTarget){
            Wander();
            _elapsedTime = 0;
        }

		if(AttemptArrival())
		{
            //_elapsedTime = 0;
            ResetWaitTimer();
        }
        //order may be important, if we force arrival
        CalculateProgress();
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

	void ForceArrive()
	{
        Vector2 currentPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
		SetTarget(currentPosition);
	}

    void OnCollisionStay2D(Collision2D info)
    {
        if (_hasTarget)
        {
            //just so we dont jump to conclusions, wait a 100 milliseconds
            if (_elapsedTime > 0.1f)
                ForceArrive();
            if (info.gameObject.name == "Beer")
            {
                RecieveBeer();
            }
        }
    }

    void RecieveBeer()
    {
		if(_hasBeer)
			return;

        _hasBeer = true;
		_hasFun = true;
        _beerIcon.SetActive(true);
		GameManager.instance.AddBeer (1);
    }

    void ResetWaitTimer()
    {
        //if we barely made progress we should get a headstart on waiting for the next move
        float inverseProgress = 1 - _wanderProgress;
        Debug.Log("Progress: " + _wanderProgress + ", HeadStart: " + inverseProgress);
        float dampenHeadStart = 0.5f;
        _elapsedTime = Mathf.Max(inverseProgress * waitTargetTime * dampenHeadStart, 0);
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