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
	private float buildingLevel = 0;
	
	private float buildingCooldown = 0.1F;
	public float lastBuildingUse;
	public bool is_buildingReady = true;
	private Transform progressBar;
	
	// Use this for initialization
	void Start () {
		progressBar = hudSlot.FindChild("ProgressBar");
		progressBar.GetComponent<ProgressBar>().measure = 1;
		progressBar.GetComponent<ProgressBar>().measureCap = 1;
		lastBuildingUse = Time.time - buildingCooldown;
	}
	
	// Update is called once per frame
	void Update () {
		BuildingReady();
	}
	
	public void BuildingReady () {
		if(buildingCooldown + lastBuildingUse < Time.time){
			is_buildingReady = true;
			progressBar.GetComponent<ProgressBar>().measure = buildingCooldown;
		}
		else{
			progressBar.GetComponent<ProgressBar>().measure = Time.time - lastBuildingUse;
		}
	}
	
	public void Selected() {
		transform.FindChild("Bracket").renderer.enabled = true;
		transform.FindChild("Bracket").GetChild(0).renderer.enabled = true;
		transform.FindChild("Bracket").GetChild(1).renderer.enabled = true;
		hudSlot.renderer.material = selectedMaterial;
	}
	
	public void UnSelected() {
		transform.FindChild("Bracket").renderer.enabled = false;
		transform.FindChild("Bracket").GetChild(0).renderer.enabled = false;
		transform.FindChild("Bracket").GetChild(1).renderer.enabled = false;
		hudSlot.renderer.material = unSelectedMaterial;
	}
	
	public void Upgrade() {
		if(thisType == BuildingType.Empty){
			hudSlot.GetComponent<HUDSlot>().ScrollOnSelect();
		}
		else{
			if(transform.parent.GetComponent<PlanetaryControls>().playerMoney > Mathf.RoundToInt((float) hudSlot.GetComponent<HUDSlot>().selectedType * Mathf.Pow(1.5F, buildingLevel))){
				Construct(hudSlot.GetComponent<HUDSlot>().selectedType);
				transform.parent.GetComponent<PlanetaryControls>().playerMoney -= Mathf.RoundToInt((float) hudSlot.GetComponent<HUDSlot>().selectedType * Mathf.Pow(1.5F, buildingLevel));
				transform.parent.GetComponent<PlanetaryControls>().moneyText.text = transform.parent.GetComponent<PlanetaryControls>().playerMoney.ToString();
				buildingLevel ++;
			}
			else{
				NotEnoughFunds();
			}
		}
	}
	
	void NotEnoughFunds () {
		Debug.Log ("not enough funds");
	}
	
	public void Construct() {
		if(thisType == BuildingType.Empty){
			if(transform.parent.GetComponent<PlanetaryControls>().playerMoney > (int) hudSlot.GetComponent<HUDSlot>().selectedType){
				Construct(hudSlot.GetComponent<HUDSlot>().selectedType);
				buildingLevel ++;
				hudSlot.GetComponent<HUDSlot>().Construct();
				transform.parent.GetComponent<PlanetaryControls>().playerMoney -= (int) hudSlot.GetComponent<HUDSlot>().selectedType;
				transform.parent.GetComponent<PlanetaryControls>().moneyText.text = transform.parent.GetComponent<PlanetaryControls>().playerMoney.ToString();
			}
			else{
				NotEnoughFunds();
			}
		}
	}	

	
	public void Construct(BuildingType type){
		thisType = type;
		if(type == BuildingType.Empty){
			buildingLevel = 0;
		}
	}
	
	public void Action() { 
		if(is_buildingReady){
			progressBar.GetComponent<ProgressBar>().measure = 1;
			progressBar.GetComponent<ProgressBar>().measureCap = 1;		
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
			progressBar.GetComponent<ProgressBar>().measureCap = buildingCooldown;	
		}
	}
		
	void FireCannon () {
		buildingCooldown = 1;	
		GameObject newProjectile = (GameObject) Instantiate(cannon1, transform.position + (transform.position - transform.parent.position) * 1.3F * (1 + (0.1F * buildingLevel)), Quaternion.LookRotation(transform.up));
		newProjectile.transform.localScale *= (1 + (0.5F * (buildingLevel - 1)));
	}
	
	void FireRocket () {
		buildingCooldown = 3;
		GameObject newProjectile = (GameObject) Instantiate(rocket1, transform.position + (transform.position - transform.parent.position) * 1.3F * (1 + (0.1F * buildingLevel)), Quaternion.LookRotation(transform.up));
		newProjectile.transform.localScale *= (1 + (0.5F * (buildingLevel - 1)));
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
		GameObject newProjectile = (GameObject) Instantiate(laser1, transform.position + (transform.position - transform.parent.position) * 1.3F * (1 + (0.25F * buildingLevel)), Quaternion.LookRotation(transform.up));
		newProjectile.transform.localScale *= (1 + (0.5F * (buildingLevel - 1)));
	}
	
	public void Reset () {
		progressBar.GetComponent<ProgressBar>().measure = 1;
		progressBar.GetComponent<ProgressBar>().measureCap = 1;
		Construct(BuildingType.Empty);
		hudSlot.GetComponent<HUDSlot>().Reset();
	}
}
