using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour {
	
	public GameObject planet1;
	public GameObject planet2;
	public Transform projectiles;
	
	// Use this for initialization
	void Start () {
	
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
