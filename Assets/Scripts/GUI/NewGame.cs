using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour {
	
	private GameObject heavenlyBodies;
	public Transform projectiles;
	static public bool is_gameStarted = false;
	static public int readyCount = 0;
	
	// Use this for initialization
	void Start () {
		heavenlyBodies = GameObject.Find ("HeavenlyBodies");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Tap () {
		for(int i = 0; i < heavenlyBodies.transform.childCount; i ++){
			heavenlyBodies.transform.GetChild(i).SendMessage("Reset", SendMessageOptions.DontRequireReceiver);
		}
		transform.parent.GetComponent<Dialog>().CloseDialog();
		for (int i = 0; i < projectiles.childCount; i++){
			Destroy(projectiles.GetChild(i).gameObject);
		}
		is_gameStarted = true;
		readyCount = 0;
	}
}
