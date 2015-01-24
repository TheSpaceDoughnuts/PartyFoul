using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	private static GameManager _instance;

	public Player player;
	public int room;
	public const float HALF_HEIGHT = 3.5f;
	public const float HALF_WIDTH = 6.5f;

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
			DontDestroyOnLoad(this);
		}
		else {
			if(this != _instance) {
				Destroy(this.gameObject);
			}
		}
	}
}
