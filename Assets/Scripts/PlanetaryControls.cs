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
	
	public int planetaryHealth = 100;
	public TextMesh healthText;
	public int playerMoney = 0;
	public TextMesh moneyText;
	private float moneyRate = 1;
	private float lastMoneyTime;
	public int player;
	public int otherPlayer;
	private Vector3 startPosition;
	
	// Use this for initialization
	void Start () {
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
			selected.GetComponent<Building>().Action();
		}
		if(Input.GetKeyDown(upgrade)){
			selected.GetComponent<Building>().Upgrade();
		}
		if(Input.GetKeyDown(construct)){
			selected.GetComponent<Building>().Construct();
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
		planetaryHealth -= collision.transform.GetComponent<Projectile>().damage;
		healthText.text = planetaryHealth.ToString();
		if(planetaryHealth < 1){
			GameState.gameOver = true;
			GameObject.Find ("GameOverMenu").GetComponent<Dialog>().OpenDialog("the winner is Player " + otherPlayer + "!");
		}
	}
	
	public void Reset () {
		planetaryHealth = 100;
		healthText.text = planetaryHealth.ToString();
		lastMoneyTime = Time.time;
		playerMoney = 0;
		transform.position = startPosition;
	}
}
