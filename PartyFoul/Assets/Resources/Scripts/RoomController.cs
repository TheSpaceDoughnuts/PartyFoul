using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RoomController : MonoBehaviour {
	
	private List<GameObject> _normals;
	private List<GameObject> _kegs;

	private bool _loaded;

	public string roomName;
	public int normalNPCCount = 5;

	public GameObject normalNPC;
	private Vector3 _cameraPosition;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnLevelWasLoaded(int level) {

		if(Application.loadedLevelName == roomName) {
			_cameraPosition = Camera.main.transform.position;
			if(!_loaded) {
				_normals = new List<GameObject>();
				_kegs = new List<GameObject>();

				for(int i = 0; i < normalNPCCount; i++) {
					Vector3 position = new Vector3();
					position.x = Random.Range (-GameManager.HALF_WIDTH/2, GameManager.HALF_WIDTH/2);
					position.y = Random.Range (-GameManager.HALF_HEIGHT/2, GameManager.HALF_HEIGHT/2);
					position.z = position.y;
					GameObject normal = Instantiate(normalNPC, position + _cameraPosition, Quaternion.identity) as GameObject;
					//normal.renderer.material.mainTexture = 
					normal.GetComponent <SpriteRenderer>().sprite = GameManager.instance.possibleNPCTexture[Random.Range (0, GameManager.instance.possibleNPCTexture.Count-1)];
					DontDestroyOnLoad(normal);
					_normals.Add (normal);
				}
				_loaded = true;
			} else {
				Debug.Log ("Beers: " + GetBeerCount());
			}
		}
	}

	public int GetHavingFunCount() {
		int count  = 0;
		foreach(GameObject npc in _normals) {
			if(npc.GetComponent<NPC_Normal>().HasFun)
				count++;
		}
		return count;
	}

	public int GetNPCCount() {
		return _normals.Count;
	}

	public int GetBeerCount() {
		int count  = 0;
		foreach(GameObject npc in _normals) {
			if(npc.GetComponent<NPC_Normal>().HasBeer)
				count++;
		}
		return count;
	}

	public List<GameObject> GetNormalNPCs() {
		return _normals;
	}
}
