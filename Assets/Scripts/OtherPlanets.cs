using UnityEngine;
using System.Collections;

public class OtherPlanets : MonoBehaviour {
	
	private Vector3 startPosition;
	public float orbitSpeed = 0.5F;
	
	// Use this for initialization
	void Start () {
		startPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}	
	
	void FixedUpdate(){
		transform.RotateAround(transform.parent.position, new Vector3(0,0,1), orbitSpeed);
	}
	
	void Reset(){
		transform.position = startPosition;
	}
}
