using UnityEngine;
using System.Collections;

public struct GameStateValues{
	public int money, upLevel, downLevel, leftLevel, rightLevel, selected, upType, downType, leftType, rightType;
	public float health;
}

public class PlanetaryControls: MonoBehaviour {
	
	public GameStateValues thisGSV;
	private string[] selectionOptions = {"up", "down", "left", "right"};
	private BuildingType[] buildingTypes = {BuildingType.Empty, BuildingType.Cannon, BuildingType.Rocket, BuildingType.Laser, BuildingType.Factory, BuildingType.Repair};
	
	public string action;
	public string upgrade;
	public string construct;
	public string switchClock;
	public string switchCounterClock;
	
	private Transform up;
	private Transform down;
	private Transform left;
	private Transform right;
	private Transform selected;
	
	public Material selectedMaterial;
	public Material unselectedMaterial;
	
	public float orbitSpeed;
	public float revolutionSpeed;
	
	public float planetaryHealth = 100;
	public TextMesh healthText;
	public Transform healthBar;
	public int playerMoney = 0;
	public TextMesh moneyText;
	private float moneyRate = 1;
	private float lastMoneyTime;
	public int player;
	public int otherPlayer;
	private Vector3 startPosition;
	private bool is_remote = false;
	private bool is_client = false;
	
	// Use this for initialization
	void Start () {
		healthBar.GetComponent<ProgressBar>().measureCap = planetaryHealth;
		healthBar.GetComponent<ProgressBar>().measure = planetaryHealth;
		startPosition = transform.position;
		lastMoneyTime = 0;
		if(transform.position.x < 0){
			player = 1;
			otherPlayer = 2;
		}
		else{
			player = 2;
			otherPlayer = 1;
		}
		up = transform.FindChild("Up");
		down = transform.FindChild("Down");
		left = transform.FindChild("Left");
		right = transform.FindChild("Right");
		selected = up;
	}
	
	// Update is called once per frame
	void Update () {
		Income();
		Action();
		ChangeSelected();
	}
	
	public void Remote(string remoteValue){
		is_remote = true;
		if(remoteValue == "client"){
			is_client = true;
		}
	}
	
	void Income(){
		if(lastMoneyTime + moneyRate < Time.time){
			playerMoney++;
			moneyText.text = playerMoney.ToString();
			lastMoneyTime = Time.time;
		}
	}
	
	void FixedUpdate(){
		transform.Rotate(new Vector3(0, 0, revolutionSpeed));
		transform.RotateAround(transform.parent.position, new Vector3(0,0,1), orbitSpeed);
	}
	
	void Action() { 
		if(Input.GetKeyDown(action)){
			GetAction ();
		}
		if(Input.GetKeyDown(upgrade)){
			selected.GetComponent<Building>().Upgrade();
		}
		if(Input.GetKeyDown(construct)){
			selected.GetComponent<Building>().Construct();
		}
	}
	
	[RPC] void GetAction () {
		selected.GetComponent<Building>().Action();
		if(!is_remote){
			networkView.RPC("GetAction", RPCMode.Server);
		}
	}
	
	void ChangeSelected(){
		if(Input.GetKeyDown(switchClock)){
			selected.GetComponent<Building>().UnSelected();
			if(selected == up){
				selected = right;
			}
			else if(selected == right){
				selected = down;
			}
			else if(selected == down){
				selected = left;
			}
			else if(selected == left){
				selected = up;
			}
		selected.GetComponent<Building>().Selected();
		}
		if(Input.GetKeyDown(switchCounterClock)){
			selected.GetComponent<Building>().UnSelected();
			if(selected == up){
				selected = left;
			}
			else if(selected == left){
				selected = down;
			}
			else if(selected == down){
				selected = right;
			}
			else if(selected == right){
				selected = up;
			}
		}
		selected.GetComponent<Building>().Selected();
	}
	
	void OnCollisionEnter(Collision collision){
		if((player == 1 && !is_remote) ||(player == 2 && is_remote)){
			BulletCollision (collision);
		}
	}
	
	[RPC] void BulletCollision (Collision collision) {
		planetaryHealth -= collision.transform.GetComponent<Projectile>().damage;
		healthText.text = planetaryHealth.ToString();
		healthBar.GetComponent<ProgressBar>().measure = planetaryHealth;
		if(planetaryHealth < 1){
			GameState.gameOver = true;
			GameObject.Find ("GameOverMenu").GetComponent<Dialog>().OpenDialog("the winner is Player " + otherPlayer + "!");
		}
		if((player == 1 && !is_remote) || (player == 2 && is_remote)){
			
		}
	}
	
	public void Reset () {
		planetaryHealth = 100;
		healthBar.GetComponent<ProgressBar>().measureCap = planetaryHealth;
		healthBar.GetComponent<ProgressBar>().measure = planetaryHealth;
		up.GetComponent<Building>().Reset();
		down.GetComponent<Building>().Reset();
		left.GetComponent<Building>().Reset();
		right.GetComponent<Building>().Reset();
		healthText.text = planetaryHealth.ToString();
		lastMoneyTime = Time.time;
		playerMoney = 0;
		transform.position = startPosition;
	}
	
	void UpdateGSV (GameStateValues newGSV) {
		if(newGSV.money != thisGSV.money){
			Debug.Log ("spent money");
		}
		if(newGSV.upLevel != thisGSV.upLevel){
			Debug.Log ("changed up");
		}
		if(newGSV.downLevel != thisGSV.downLevel){
			Debug.Log ("changed down");
		}
		if(newGSV.rightLevel != thisGSV.rightLevel){
			Debug.Log ("changed right");
		}
		if(newGSV.leftLevel != thisGSV.leftLevel){
			Debug.Log ("changed left");
		}
		if(newGSV.health != thisGSV.health){
			Debug.Log ("changed health");
		}
		if(newGSV.selected != thisGSV.selected){
			Debug.Log ("changed selected");
		}
		if(newGSV.upType != thisGSV.upType){
			Debug.Log ("changed uptype");
		}
		if(newGSV.downType != thisGSV.downType){
			Debug.Log ("changed downtype");
		}
		if(newGSV.rightType != thisGSV.rightType){
			Debug.Log ("changed righttype");
		}
		if(newGSV.leftType != thisGSV.leftType){
			Debug.Log ("changed lefttype");
		}
	}
	
	void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
	{
		GameStateValues syncGSV = new GameStateValues();
	    if (stream.isWriting)
	    {
	        syncGSV = thisGSV;
	        stream.Serialize(ref syncGSV.money);
	        stream.Serialize(ref syncGSV.upLevel);
	        stream.Serialize(ref syncGSV.downLevel);
	        stream.Serialize(ref syncGSV.rightLevel);
	        stream.Serialize(ref syncGSV.leftLevel);
	        stream.Serialize(ref syncGSV.health);
	        stream.Serialize(ref syncGSV.selected);
	        stream.Serialize(ref syncGSV.upType);
	        stream.Serialize(ref syncGSV.downType);
	        stream.Serialize(ref syncGSV.rightType);
	        stream.Serialize(ref syncGSV.leftType);
	    }
	    else
	    {
	        stream.Serialize(ref syncGSV.money);
	        stream.Serialize(ref syncGSV.upLevel);
	        stream.Serialize(ref syncGSV.downLevel);
	        stream.Serialize(ref syncGSV.rightLevel);
	        stream.Serialize(ref syncGSV.leftLevel);
	        stream.Serialize(ref syncGSV.health);
	        stream.Serialize(ref syncGSV.selected);
	        stream.Serialize(ref syncGSV.upType);
	        stream.Serialize(ref syncGSV.downType);
	        stream.Serialize(ref syncGSV.rightType);
	        stream.Serialize(ref syncGSV.leftType);
			UpdateGSV(syncGSV);
			thisGSV = syncGSV;
	    }
	}
}
