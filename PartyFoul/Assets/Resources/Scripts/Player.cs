using UnityEngine;
using System.Collections;

public class Player : PartyPerson {

	private Animator _animator;
	private int _facing = 0;

	// Use this for initialization
	protected override void OnStart () {
		base.OnStart ();

		_animator = this.GetComponent<Animator>();
	}
	
	protected override void OnUpdate ()
	{
		base.OnUpdate ();
		#region WASD Code
		Vector2 input = new Vector2();
		if(Input.GetKey(KeyCode.W)) {
			input.y += force.y;
			_facing = 2;
		}
		if(Input.GetKey(KeyCode.S)){
			input.y -= force.y;
			_facing = 0;
		}
		if(Input.GetKey(KeyCode.A)) {
			input.x -= force.x;
			_facing = 1;
		}
		if(Input.GetKey(KeyCode.D)) {
			input.x += force.x;
			_facing = 3;
		}
		_animator.SetInteger("Direction", _facing);

		gameObject.rigidbody2D.AddForce (input);
		#endregion
		
		if(Input.GetMouseButtonDown(1)) {
			//1280x720
			Vector2 target = Input.mousePosition;
			
			Vector2 rescale = new Vector2(target.x / Screen.width, 
			                              target.y / Screen.height);
			Vector2 size = new Vector2(GameManager.HALF_WIDTH, GameManager.HALF_HEIGHT);
			Vector2 result = new Vector2(rescale.x * size.x*2,
			                             rescale.y * size.y*2);
			result.x -= size.x;
			result.y -= size.y;
			
			SetTarget (result);
		}
		
		AttemptArrival();
	}
}
