using UnityEngine;
using System.Collections;

public class PreventDestroy : MonoBehaviour {

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad(this);
	}
}
