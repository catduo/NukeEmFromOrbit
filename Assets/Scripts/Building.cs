using UnityEngine;
using System.Collections;

public enum BuildingType{
	Empty = 0,
	Factory = 5,
	Repair = 30,
	Rocket = 25,
	Cannon = 15,
	Laser = 10
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
	public GameObject cannon1;
	public GameObject rocket1;
	public GameObject laser1;
	private Vector3 fireDirection;
	private BuildingType thisType = BuildingType.Empty;
	private int buildingLevel;
	
	private float buildingCooldown = 0;
	public float lastBuildingUse;
	public bool is_buildingReady = true;
	
	// Use this for initialization
	void Start () {
		lastBuildingUse = Time.time - buildingCooldown;
	}
	
	// Update is called once per frame
	void Update () {
		BuildingReady();
	}
	
	public void BuildingReady () {
		if(buildingCooldown + lastBuildingUse < Time.time){
			is_buildingReady = true;
		}
	}
	
	public void Selected() {
		transform.GetChild(0).renderer.material = selectedMaterial;
		hudSlot.renderer.material = selectedMaterial;
	}
	
	public void UnSelected() {
		transform.GetChild(0).renderer.material = unSelectedMaterial;
		hudSlot.renderer.material = unSelectedMaterial;
	}
	
	public void Upgrade() {
		if(thisType == BuildingType.Empty){
			hudSlot.GetComponent<HUDSlot>().ScrollOnSelect();
		}
	}
	
	public void Construct() {
		if(transform.parent.GetComponent<PlanetaryControls>().playerMoney > (int) hudSlot.GetComponent<HUDSlot>().selectedType){
			Construct(hudSlot.GetComponent<HUDSlot>().selectedType);
		}
		else{
			NotEnoughFunds();
		}
	}	
	
	void NotEnoughFunds () {
		Debug.Log ("not enough funds");
	}
	
	public void Construct(BuildingType type){
		thisType = type;
	}
	
	public void Action() { 
		if(is_buildingReady){
			is_buildingReady = false;
			lastBuildingUse = Time.time;
			switch(thisType){
			case BuildingType.Cannon:
				FireCannon();
				break;
				
			case BuildingType.Factory:
				CollectFactory();
				break;
				
			case BuildingType.Laser:
				FireLaser();
				break;
				
			case BuildingType.Repair:
				RepairPlanet();
				break;
				
			case BuildingType.Rocket:
				FireRocket();
				break;
				
			default:
				break;
			}
		}
	}
		
	void FireCannon () {
		buildingCooldown = 1;
		GameObject newProjectile = (GameObject) Instantiate(cannon1, transform.position * 2.3F - transform.parent.position * 1.3F, Quaternion.LookRotation(transform.up));
	}
	
	void FireRocket () {
		buildingCooldown = 3;
		GameObject newProjectile = (GameObject) Instantiate(rocket1, transform.position * 2.3F - transform.parent.position * 1.3F, Quaternion.LookRotation(transform.up));
	}
	
	void CollectFactory () {
		buildingCooldown = 5;
		transform.parent.GetComponent<PlanetaryControls>().playerMoney += 5;
		transform.parent.GetComponent<PlanetaryControls>().moneyText.text = transform.parent.GetComponent<PlanetaryControls>().playerMoney.ToString();
	}
	
	void RepairPlanet () {
		buildingCooldown = 10;
		transform.parent.GetComponent<PlanetaryControls>().planetaryHealth += 5;
		transform.parent.GetComponent<PlanetaryControls>().healthText.text = transform.parent.GetComponent<PlanetaryControls>().planetaryHealth.ToString();
	}
	
	void FireLaser () {
		buildingCooldown = 0;
		GameObject newProjectile = (GameObject) Instantiate(laser1, transform.position * 2.3F - transform.parent.position * 1.3F, Quaternion.LookRotation(transform.up));
	}
}
