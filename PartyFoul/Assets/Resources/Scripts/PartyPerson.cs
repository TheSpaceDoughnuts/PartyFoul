using UnityEngine;
using System.Collections;

public class PartyPerson : MonoBehaviour {
	
	protected Vector2 _target = Vector2.zero;
	protected bool _hasTarget = false;
	protected SpriteRenderer _spriteRenderer;
	
	public Vector2 force;
	public Vector3 scale;
	public float arrivalDistance = 0.1f;
	private const float DEPTH_PRECISION = 100.0f;

	void Start () {
		OnStart ();
	}
	
	// Use this for initialization
	protected virtual void OnStart() {
		_spriteRenderer = GetComponent<SpriteRenderer>();
		gameObject.transform.localScale = scale;
	}
	
	// Update is called once per frame
	void Update () {
		//this should always be done, supposedly
		OnUpdate();
	}
	
	protected virtual void OnUpdate(){
		GameManager.DepthSort(this.gameObject);
        //Reorient();
		SeekTarget();
	}
	
	public void SetTarget(Vector2 target) {
		_target = target;
		_hasTarget = true;
	}

    /// <summary>
    /// Make sure the person is facing the way they are moving
    /// </summary>
    void Reorient()
    {
        float direction = -Mathf.Sign(rigidbody2D.velocity.x);
        if (direction == 0)
            return;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * direction, transform.localScale.y, transform.localScale.z);
    }
	
	public void SeekTarget() {
		if(_hasTarget) {
			Vector2 dir = _target;
			dir.x -= gameObject.transform.position.x;
			dir.y -= gameObject.transform.position.y;
			Vector2 norm = dir.normalized;
			norm.Scale(force);
			gameObject.rigidbody2D.AddForce (norm);
		}
	}
	
	public bool AttemptArrival() {
		if(_hasTarget) {
			float distance = Distance(gameObject.transform.position, _target);
			if(distance < arrivalDistance) {
				_hasTarget = false;
				return true;
			}
		}
		return false;
	}
	
	public float Distance(Vector2 start, Vector2 end) {
        float dx = (end.x - start.x);
        float dy = (end.y - start.y);
		
		float distance = Mathf.Sqrt (dx * dx + dy * dy);
		
		return distance;
	}
}
