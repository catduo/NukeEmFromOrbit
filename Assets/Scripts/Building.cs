using UnityEngine;
using System.Collections;

public enum BuildingType{
	Empty,
	Factory,
	Repair,
	Rocket,
	Cannon,
	Laser
}

public struct BuildingStatus{
	public float cooldownTime, lastFiredTime;
	public int level, upgradeCost, damage;
	public bool ready;
}

public class Building : MonoBehaviour {
	
	public Transform hudSlot;
	public Material selectedMaterial;
	public Material unSelectedMaterial;	
	public GameObject projectile1;
	private Vector3 fireDirection;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Selected() {
		renderer.material = selectedMaterial;
		hudSlot.renderer.material = selectedMaterial;
	}
	
	public void UnSelected() {
		renderer.material = unSelectedMaterial;
		hudSlot.renderer.material = unSelectedMaterial;
	}
	
	public void Upgrade() {
		Debug.Log ("upgrade");
	}
	
	public void Construct() {
		Debug.Log ("construct");
	}	
	
	public void Action() { 
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
		GameObject newProjectile = (GameObject) Instantiate(projectile1, transform.position * 2.3F - transform.parent.position * 1.3F, Quaternion.LookRotation(fireDirection));
	}
}
