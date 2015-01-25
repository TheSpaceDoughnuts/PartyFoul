using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	public Player player;
	public Text beerText, funText;

	public string lastRoom = "MainMenu";
	public const float HALF_HEIGHT = 3.5f;
	public const float HALF_WIDTH = 6.5f;

	public float currentBeer = 0.0f;
	public float maxBeer = 100.0f;

	public List<RoomController> rooms;
	public List<Sprite> possibleNPCTexture;

	public static GameManager instance {
		get {
			if(_instance == null) {
				_instance = GameObject.FindObjectOfType<GameManager>();

				// Prevents destroying between scenes
				DontDestroyOnLoad(_instance.gameObject);
			}

			return _instance;
		}
	}
	
	void Awake() {
		if(_instance == null) {
			_instance = this;
			_instance.lastRoom = "MainMenu";
			UpdateBeerText ();
			UpdateFunText ();
			DontDestroyOnLoad(this);
		}
		else {
			if(this != _instance) {
				Destroy(this.gameObject);
			}
		}
	}

	void Update() {
			
	}

	public static void DepthSort(GameObject target) {
		float y = target.transform.position.y;
		if(target.collider != null){
			y = target.collider.bounds.min.y;
		}
		y -= Camera.main.transform.position.y;
		//the higher up an object is the further it is into the background
		target.transform.position = new Vector3(target.transform.position.x,
		                                        target.transform.position.y,
		                                        y - GameManager.HALF_HEIGHT);
	}

	public void UpdateBeerText() {
		instance.beerText.text = "Beer: " + ((instance.currentBeer/instance.maxBeer)*100) + "%";
	}

	public void UpdateFunText() {
		//instance.funText.text = "Fun: " + ((instance.currentFun/instance.maxFun)*100) + "%";
	}

	public void AddBeer(float beerAmount) {
		instance.currentBeer += beerAmount;
		if(instance.currentBeer > instance.maxBeer)
			instance.currentBeer = instance.maxBeer;

		UpdateBeerText ();
	}
}
