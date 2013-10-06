using UnityEngine;
using System.Collections;

public class HUDSlot : MonoBehaviour {
	
	public Transform cannonBuilding;
	public Transform rocketBuilding;
	public Transform laserBuilding;
	public Transform factoryBuilding;
	public Transform repairBuilding;
	public Transform Building;
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
			Transform buildingListObject = (Transform) Transform.Instantiate(buildingList[i], new Vector3(transform.position.x + 1 * i - 2.5F, transform.position.y, transform.position.z), transform.rotation);
			buildingListObject.localScale = new Vector3(1 / Mathf.Sqrt(i + 1), 1 / Mathf.Sqrt(i + 1), 1 / Mathf.Sqrt(i + 1));
			buildingListObject.parent = transform;
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
		for(int i = 0; i < buildingList.Length; i ++){
			Destroy(buildingList[i].gameObject);
		}
		for( int i = 0; i < transform.FindChild("ProgressBar").childCount; i++){
			transform.FindChild("ProgressBar").GetChild(i).renderer.enabled = true;
		}
	}
	
	public void ScrollOnSelect(){
		Debug.Log (buildingList[0].name);
		buildingList = new Transform[] {buildingList[1], buildingList[2], buildingList[3], buildingList[4], buildingList[0]};
		buildingTypeList = new BuildingType[] {buildingTypeList[1], buildingTypeList[2], buildingTypeList[3], buildingTypeList[4], buildingTypeList[0]};
		for(int i = 0; i < buildingList.Length; i++){
			buildingList[i].position = new Vector3(transform.position.x + 1 * i - 2.5F, transform.position.y, transform.position.z);
			buildingList[i].localScale = new Vector3(1 / Mathf.Sqrt(i + 1), 1 / Mathf.Sqrt(i + 1), 1 / Mathf.Sqrt(i + 1));
		}
		selectedType = buildingTypeList[0];
	}
	
	public void Reset(){
		buildingList = new Transform[] {cannonBuilding, rocketBuilding, laserBuilding, factoryBuilding, repairBuilding};
		buildingTypeList = new BuildingType[] {BuildingType.Cannon, BuildingType.Rocket, BuildingType.Laser, BuildingType.Factory, BuildingType.Repair};
		for(int i = 0; i < buildingList.Length; i++){
			Transform buildingListObject = (Transform) Transform.Instantiate(buildingList[i], new Vector3(transform.position.x + 1 * i - 2.5F, transform.position.y, transform.position.z), transform.rotation);
			buildingListObject.localScale = new Vector3(1 / Mathf.Sqrt(i + 1), 1 / Mathf.Sqrt(i + 1), 1 / Mathf.Sqrt(i + 1));
			buildingListObject.parent = transform;
		}
		selectedType = buildingTypeList[0];
		for( int i = 0; i < transform.FindChild("ProgressBar").childCount; i++){
			transform.FindChild("ProgressBar").GetChild(i).renderer.enabled = false;
		}
	}
}
