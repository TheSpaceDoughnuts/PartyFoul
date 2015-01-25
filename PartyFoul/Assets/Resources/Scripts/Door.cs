﻿using UnityEngine;
using System.Collections;

public class Door : MonoBehaviour {

	public string targetRoom;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col) {
		if(col.gameObject.tag == "Player") {
			GameManager.instance.lastRoom = Application.loadedLevelName;
			Application.LoadLevel (targetRoom);
		}
	}
}
