using UnityEngine;
using System.Collections;

public class PartyPerson : MonoBehaviour {

	private Vector2 _target = Vector2.zero;
	private bool _hasTarget = false;

	public Vector2 force;
	public Vector3 scale;
	public float arrivalDistance = 0.1f;

	// Use this for initialization
	void Start () {
		OnStart ();
	}

	protected virtual void OnStart() {
		gameObject.transform.localScale = scale;
	}

	// Update is called once per frame
	void Update () {
		OnUpdate();
	}

	protected virtual void OnUpdate(){
		SeekTarget();

		if(_hasTarget) {
			float distance = Distance(gameObject.transform.position, _target);
			if(distance < arrivalDistance) {
				_hasTarget = false;
			}
		}
	}

	public void SetTarget(Vector2 target) {
		_target = target;
		_hasTarget = true;
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

	public float Distance(Vector2 start, Vector2 end) {
		float dx = (end.x - start.x) * (end.x - start.x);
		float dy = (end.y - start.y) * (end.y - start.y);

		float distance = Mathf.Sqrt (dx + dy);

		return distance;
	}
}
