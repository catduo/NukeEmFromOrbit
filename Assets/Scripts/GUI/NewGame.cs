using UnityEngine;
using System.Collections;

public class NewGame : MonoBehaviour {
	
	public GameObject planet1;
	public GameObject planet2;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void Tap () {
		planet1.GetComponent<PlanetaryControls>().planetaryHealth = 100;
		planet2.GetComponent<PlanetaryControls>().planetaryHealth = 100;
		planet1.GetComponent<PlanetaryControls>().healthText.text = "100";
		planet2.GetComponent<PlanetaryControls>().healthText.text = "100";
		transform.parent.GetComponent<Dialog>().CloseDialog();
	}
}
