using UnityEngine;
using System.Collections;
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

	public float currentFun = 0.0f;
	public float maxFun = 100.0f;

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
			DontDestroyOnLoad(this);
		}
		else {
			if(this != _instance) {
				Destroy(this.gameObject);
			}
		}
	}

	public static void UpdateBeerText() {
		//_instance.beerText;
	}
}
