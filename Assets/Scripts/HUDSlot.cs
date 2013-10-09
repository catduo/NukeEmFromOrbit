using UnityEngine;
using System.Collections;

public class HUDSlot : MonoBehaviour {
	
	public Transform cannonBuilding;
	public Transform rocketBuilding;
	public Transform laserBuilding;
	public Transform factoryBuilding;
	public Transform repairBuilding;
	private TextMesh title;
	private int selectedTitle = 0;
	private Transform[] buildingList;
	private BuildingType[] buildingTypeList;
	private string[] titles;
	private bool is_built;
	private bool is_selected;
	public BuildingType selectedType;
	
	// Use this for initialization
	void Start () {
		titles = new string[] {"Cannon", "Rocket", "Laser", "Factory", "Repair"};
		title = transform.FindChild("Title").GetComponent<TextMesh>();
		title.text = "Construct " + titles [selectedTitle];
		buildingList = new Transform[] {cannonBuilding, rocketBuilding, laserBuilding, factoryBuilding, repairBuilding};
		buildingTypeList = new BuildingType[] {BuildingType.Cannon, BuildingType.Rocket, BuildingType.Laser, BuildingType.Factory, BuildingType.Repair};
		for(int i = 0; i < buildingList.Length; i++){
			Transform buildingListObject = (Transform) Transform.Instantiate(buildingList[i], new Vector3(transform.position.x + 1 * i - 2F, transform.position.y - 0.05F * i, transform.position.z), transform.rotation);
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
		title.text = "Trigger " + titles [selectedTitle];
	}
	
	public void ScrollOnSelect(int direction){
		selectedTitle += direction;
		if(selectedTitle > 4){
			selectedTitle = 0;
		}
		else if(selectedTitle < 0){
			selectedTitle = 4;
		}
		title.text = "Construct " + titles [selectedTitle];
		if(direction > 0){
			buildingList = new Transform[] {buildingList[1], buildingList[2], buildingList[3], buildingList[4], buildingList[0]};
			buildingTypeList = new BuildingType[] {buildingTypeList[1], buildingTypeList[2], buildingTypeList[3], buildingTypeList[4], buildingTypeList[0]};
		}
		else{
			buildingList = new Transform[] {buildingList[4], buildingList[0], buildingList[1], buildingList[2], buildingList[3]};
			buildingTypeList = new BuildingType[] {buildingTypeList[4], buildingTypeList[0], buildingTypeList[1], buildingTypeList[2], buildingTypeList[3]};
		}
		for(int i = 0; i < buildingList.Length; i++){
			buildingList[i].position = new Vector3(transform.position.x + 1 * i - 2F, transform.position.y - 0.05F * i, transform.position.z);
			buildingList[i].localScale = new Vector3(1 / Mathf.Sqrt(i + 1)/ transform.lossyScale.x, 1 / Mathf.Sqrt(i + 1)/transform.lossyScale.y, 1 / Mathf.Sqrt(i + 1)/transform.lossyScale.z);
		}
		selectedType = buildingTypeList[0];
	}
	
	public void Reset(){
		buildingList = new Transform[] {cannonBuilding, rocketBuilding, laserBuilding, factoryBuilding, repairBuilding};
		buildingTypeList = new BuildingType[] {BuildingType.Cannon, BuildingType.Rocket, BuildingType.Laser, BuildingType.Factory, BuildingType.Repair};
		for(int i = 0; i < buildingList.Length; i++){
			Transform buildingListObject = (Transform) Transform.Instantiate(buildingList[i], new Vector3(transform.position.x + 1 * i - 2F, transform.position.y - 0.05F * i, transform.position.z), transform.rotation);
			buildingListObject.parent = transform;
			buildingListObject.localScale = new Vector3(1 / Mathf.Sqrt(i + 1)/ transform.lossyScale.x, 1 / Mathf.Sqrt(i + 1)/transform.lossyScale.y, 1 / Mathf.Sqrt(i + 1)/transform.lossyScale.z);
		}
		selectedType = buildingTypeList[0];
		for( int i = 0; i < transform.FindChild("ProgressBar").childCount; i++){
			transform.FindChild("ProgressBar").GetChild(i).renderer.enabled = false;
		}
	}
}
