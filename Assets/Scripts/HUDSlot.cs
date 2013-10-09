﻿using UnityEngine;
using System.Collections;

public class HUDSlot : MonoBehaviour {
	
	public Transform cannonBuilding;
	public Transform rocketBuilding;
	public Transform laserBuilding;
	public Transform factoryBuilding;
	public Transform repairBuilding;
	private Transform[] buildingList;
	private BuildingType[] buildingTypeList;
	private bool is_built;
	private bool is_selected;
	public BuildingType selectedType;
	
	// Use this for initialization
	void Start () {
		buildingList = new Transform[] {cannonBuilding, rocketBuilding, laserBuilding, factoryBuilding, repairBuilding};
		buildingTypeList = new BuildingType[] {BuildingType.Cannon, BuildingType.Rocket, BuildingType.Laser, BuildingType.Factory, BuildingType.Repair};
		for(int i = 0; i < buildingList.Length; i++){
			Transform buildingListObject = (Transform) Transform.Instantiate(buildingList[i], new Vector3(transform.position.x + 1 * i - 2.5F, transform.position.y - 0.05F * i, transform.position.z), transform.rotation);
			buildingListObject.parent = transform;
			buildingListObject.localScale = new Vector3(1 / Mathf.Sqrt(i + 1)/ transform.lossyScale.x, 1 / Mathf.Sqrt(i + 1)/transform.lossyScale.y, 1 / Mathf.Sqrt(i + 1)/transform.lossyScale.z);
			buildingList[i] = buildingListObject;
		}
		selectedType = buildingTypeList[0];
		for( int i = 0; i < transform.FindChild("ProgressBar").childCount; i++){
			transform.FindChild("ProgressBar").GetChild(i).renderer.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void Construct () {
		for(int i = 1; i < buildingList.Length; i ++){
			Destroy(buildingList[i].gameObject);
		}
		for( int i = 0; i < transform.FindChild("ProgressBar").childCount; i++){
			transform.FindChild("ProgressBar").GetChild(i).renderer.enabled = true;
		}
	}
	
	public void ScrollOnSelect(){
		buildingList = new Transform[] {buildingList[1], buildingList[2], buildingList[3], buildingList[4], buildingList[0]};
		buildingTypeList = new BuildingType[] {buildingTypeList[1], buildingTypeList[2], buildingTypeList[3], buildingTypeList[4], buildingTypeList[0]};
		for(int i = 0; i < buildingList.Length; i++){
			buildingList[i].position = new Vector3(transform.position.x + 1 * i - 2.5F, transform.position.y - 0.05F * i, transform.position.z);
			buildingList[i].localScale = new Vector3(1 / Mathf.Sqrt(i + 1)/ transform.lossyScale.x, 1 / Mathf.Sqrt(i + 1)/transform.lossyScale.y, 1 / Mathf.Sqrt(i + 1)/transform.lossyScale.z);
		}
		selectedType = buildingTypeList[0];
	}
	
	public void Reset(){
		buildingList = new Transform[] {cannonBuilding, rocketBuilding, laserBuilding, factoryBuilding, repairBuilding};
		buildingTypeList = new BuildingType[] {BuildingType.Cannon, BuildingType.Rocket, BuildingType.Laser, BuildingType.Factory, BuildingType.Repair};
		for(int i = 0; i < buildingList.Length; i++){
			Transform buildingListObject = (Transform) Transform.Instantiate(buildingList[i], new Vector3(transform.position.x + 1 * i - 2.5F, transform.position.y - 0.05F * i, transform.position.z), transform.rotation);
			buildingListObject.parent = transform;
			buildingListObject.localScale = new Vector3(1 / Mathf.Sqrt(i + 1)/ transform.lossyScale.x, 1 / Mathf.Sqrt(i + 1)/transform.lossyScale.y, 1 / Mathf.Sqrt(i + 1)/transform.lossyScale.z);
		}
		selectedType = buildingTypeList[0];
		for( int i = 0; i < transform.FindChild("ProgressBar").childCount; i++){
			transform.FindChild("ProgressBar").GetChild(i).renderer.enabled = false;
		}
	}
}
