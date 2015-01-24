using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

	private Vector2 _target = Vector2.zero;
	private bool _hasTarget = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 input = new Vector2();
		Vector2 force = new Vector2(10f, 10f);

		#region WASD Code
		/*if(Input.GetKey(KeyCode.W)) {
			input.y += force.y;
		}
		if(Input.GetKey(KeyCode.S)){
			input.y -= force.y;
		}
		if(Input.GetKey(KeyCode.A)) {
			input.x -= force.x;
		}
		if(Input.GetKey(KeyCode.D)) {
			input.x += force.x;
		}*/
		#endregion

		if(Input.GetMouseButtonDown(1)) {
			//1280x720
			_target = Input.mousePosition;
			Vector2 rescale = new Vector2(_target.x / Screen.width, 
			                              _target.y / Screen.height);
			Vector2 size = new Vector2(6.5f, 3.5f);
			Vector2 result = new Vector2(rescale.x * size.x*2,
			                             rescale.y * size.y*2);
			result.x -= size.x;
			result.y -= size.y;
			_target = result;
			_hasTarget = true;
		}

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
