using UnityEngine;
using System.Collections;

public class NPC_Normal : PartyPerson {
	Vector2 lastDirection = Vector2.zero;
	bool backOff;
	float elapsedTime;
	public float waitTargetTime = 1;

	protected override void OnUpdate ()
	{
		base.OnUpdate();

		elapsedTime += Time.deltaTime;
		if(elapsedTime > waitTargetTime && !_hasTarget){
			elapsedTime = 0;
			Wander();
		}

		if(AttemptArrival())
		{
			elapsedTime = 0;
		}
	}

	void OnCollisionEnter2D(Collision2D info)
	{
		Debug.Log("NPC Hit Wall, Bonk");
		if(!info.collider.isTrigger && info.collider.rigidbody == null)
		{
			lastDirection = -info.contacts[0].normal;
			backOff = true;
			ForceArrive();
		}
	}

	void ForceArrive()
	{
		Vector2 currentPosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y); 
		SetTarget(currentPosition);
	}

	void Wander()
	{
		float dist = 1.5f;
		Vector2 dir = Vector2.zero;
		//If we hit something or someone we want to avoid them
		Debug.Log("Backoff == " + backOff);
		if(backOff)
		{ 
			dir = -lastDirection;
			//we can chill out
			backOff = false;
		}else{
			dir = Random.insideUnitCircle;
		}
		dir.Normalize();
		WanderTowards(dir, dist);
	}
	void WanderTowards(Vector2 direction, float distance)
	{
		SetTarget(direction * distance);
		lastDirection = direction;
	}
}