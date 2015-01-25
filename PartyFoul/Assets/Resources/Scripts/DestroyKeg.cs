using UnityEngine;
using System.Collections;

public class DestroyKeg : MonoBehaviour {

	void Start() {

	}

	void Update() {

	}

	void OnMouseDown() {
		Debug.Log ("mouse down");
		Destroy (this.gameObject);
	}
}
