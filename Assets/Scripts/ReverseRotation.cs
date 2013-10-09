using UnityEngine;
using System.Collections;

public class ReverseRotation : MonoBehaviour {
	
	public float reverseSpeed;
	
	// Use this for initialization
	void Start () {
		reverseSpeed = transform.parent.GetComponent<PlanetaryControls>().revolutionSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.Rotate(0, reverseSpeed, 0);
	}
}
