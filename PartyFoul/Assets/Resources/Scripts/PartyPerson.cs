using UnityEngine;
using System.Collections;

public class PartyPerson : MonoBehaviour {

	private Vector2 _target = Vector2.zero;
	private bool _hasTarget = false;

	public Vector2 force;
	public Vector3 scale;

	// Use this for initialization
	void Start () {
		OnStart ();
	}

	protected virtual void OnStart() {
		Debug.Log ("start");
		gameObject.transform.localScale = scale;
	}

	// Update is called once per frame
	void Update () {
		OnUpdate();
	}

	protected virtual void OnUpdate(){
		SeekTarget();
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
}
