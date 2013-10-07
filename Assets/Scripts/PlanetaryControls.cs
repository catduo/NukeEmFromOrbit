using UnityEngine;
using System.Collections;

public class PlanetaryControls: MonoBehaviour {
	
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
		if(!is_remote){
			Action();
			ChangeSelected();
		}
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
			GetUpgrade ();
			selected.GetComponent<Building>().Upgrade();
		}
		if(Input.GetKeyDown(construct)){
			GetConstruct ();
		}
	}
	
	[RPC] void GetAction () {
		Debug.Log ("up");
		selected.GetComponent<Building>().Action();
		if(!is_remote){
			networkView.RPC("GetAction", RPCMode.Others);
		}
	}
	
	[RPC] void GetUpgrade () {
		selected.GetComponent<Building>().Upgrade();
		if(!is_remote){
			networkView.RPC("GetUpgrade", RPCMode.Others);
		}
	}
	
	[RPC] void GetConstruct () {
		selected.GetComponent<Building>().Construct();
		if(!is_remote){
			networkView.RPC("GetConstruct", RPCMode.Others);
		}
	}
	
	void ChangeSelected(){
		if(Input.GetKeyDown(switchClock)){
			GetChangeClock () ;
		}
		if(Input.GetKeyDown(switchCounterClock)){
			GetChangeCounterClock ();
		}
	}
	
	[RPC] void GetChangeClock () {
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
		if(!is_remote){
			networkView.RPC("GetChangeClock", RPCMode.Others);
		}
	}
	
	[RPC] void GetChangeCounterClock () {
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
		selected.GetComponent<Building>().Selected();
		if(!is_remote){
			networkView.RPC("GetChangeCounterClock", RPCMode.Others);
		}
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
			networkView.RPC("BulletCollision", RPCMode.Others, collision);
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
}
