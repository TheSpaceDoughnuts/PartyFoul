using UnityEngine;
using System.Collections;

public class NPC_Normal : PartyPerson {
	float elapsedTime;
	public float waitTargetTime = 1;

	protected override void OnUpdate ()
	{
		elapsedTime += Time.deltaTime;
		if(elapsedTime > waitTargetTime){
			elapsedTime = 0;
			SetTarget(gameObject.transform.position * -1);
		}
		
		SeekTarget();
	}
}