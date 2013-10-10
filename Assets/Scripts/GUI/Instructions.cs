using UnityEngine;
using System.Collections;

public class Instructions : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Tap () {
		GameObject.Find ("MainCamera").transform.position = new Vector3(0,0,-20);
	}
}
