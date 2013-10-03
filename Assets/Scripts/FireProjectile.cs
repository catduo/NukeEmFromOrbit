using UnityEngine;
using System.Collections;

public class FireProjectile : MonoBehaviour {
	
	public GameObject projectile1;
	private Vector3 fireDirection;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void Fire() { 
		switch(transform.name){
		case "Up":
			fireDirection = transform.parent.up;
			break;
		case "Down":
			fireDirection = -1 * transform.parent.up;
			break;
		case "Left":
			fireDirection = -1 * transform.parent.right;
			break;
		case "Right":
			fireDirection = transform.parent.right;
			break;
		default:
			Debug.Log ("directional error");
			break;
		}
		Debug.Log (transform.name);
		GameObject newProjectile = (GameObject) Instantiate(projectile1, transform.position * 2.3F - transform.parent.position * 1.3F, Quaternion.LookRotation(fireDirection));
		}
}
