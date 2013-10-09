using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour {
	
	private GameObject planet1;
	private GameObject planet2;
	public Transform projectiles;
	
	// Use this for initialization
	void Start () {
		planet1 = GameObject.Find ("Player1Planet");
		planet2 = GameObject.Find ("Player2Planet");
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void Tap () {
		planet1.GetComponent<PlanetaryControls>().Reset ();
		planet2.GetComponent<PlanetaryControls>().Reset ();
		transform.parent.GetComponent<Dialog>().CloseDialog();
		for (int i = 0; i < projectiles.childCount; i++){
			Destroy(projectiles.GetChild(i).gameObject);
		}
	}
}
