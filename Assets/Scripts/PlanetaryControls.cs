using UnityEngine;
using System.Collections;

public class PlanetaryControls: MonoBehaviour {
	
	public GameObject projectile1;
	public string fire1;
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
	public TextMesh heathText;
	public int player;
	public int otherPlayer;
	
	// Use this for initialization
	void Start () {
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
		Fire();
		ChangeCannon();
	}
	
	void FixedUpdate(){
		transform.Rotate(new Vector3(0, 0, revolutionSpeed));
		transform.RotateAround(transform.parent.position, new Vector3(0,0,1), orbitSpeed);
	}
	
	void Fire() { 
		if(Input.GetKeyDown(fire1)){
			selected.GetComponent<FireProjectile>().Fire();
		}
	}
	
	void ChangeCannon(){
		if(Input.GetKeyDown(switchClock)){
			if(selected == up){
				up.renderer.material = unselectedMaterial;
				selected = right;
			}
			else if(selected == right){
				right.renderer.material = unselectedMaterial;
				selected = down;
			}
			else if(selected == down){
				down.renderer.material = unselectedMaterial;
				selected = left;
			}
			else if(selected == left){
				left.renderer.material = unselectedMaterial;
				selected = up;
			}
		}
		if(Input.GetKeyDown(switchCounterClock)){
			if(selected == up){
				up.renderer.material = unselectedMaterial;
				selected = left;
			}
			else if(selected == left){
				left.renderer.material = unselectedMaterial;
				selected = down;
			}
			else if(selected == down){
				down.renderer.material = unselectedMaterial;
				selected = right;
			}
			else if(selected == right){
				right.renderer.material = unselectedMaterial;
				selected = up;
			}
		}
		selected.renderer.material = selectedMaterial;
	}
	
	void OnCollisionEnter(Collision collision){
		planetaryHealth -= collision.transform.GetComponent<Projectile>().damage;
		heathText.text = planetaryHealth.ToString();
		if(planetaryHealth < 1){
			GameState.gameOver = true;
			GameObject.Find ("GameOverMenu").GetComponent<Dialog>().OpenDialog("the winner is Player " + otherPlayer + "!");
		}
	}
}
