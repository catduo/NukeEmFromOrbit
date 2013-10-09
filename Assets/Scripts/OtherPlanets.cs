using UnityEngine;
using System.Collections;

public class OtherPlanets : MonoBehaviour {
	
	public float revolutionSpeed = 4F;
	public float orbitSpeed = 0.5F;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}	
	
	void FixedUpdate(){
		transform.Rotate(new Vector3(0, 0, revolutionSpeed));
		transform.RotateAround(transform.parent.position, new Vector3(0,0,1), orbitSpeed);
	}
}
