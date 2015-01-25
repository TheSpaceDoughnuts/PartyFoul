using UnityEngine;
using System.Collections;

public class BasicObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		GameManager.DepthSort (this.gameObject);
	}
}
