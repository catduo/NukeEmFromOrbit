using UnityEngine;
using System.Collections;

public class Dialog : MonoBehaviour {
	
	private GameObject mainCamera;
	public bool open;
	
	// Use this for initialization
	void Start () {
		mainCamera = GameObject.Find ("MainCamera");
		CloseDialog();
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	public void OpenDialog(){
		OpenDialog("");
	}
	
	public void OpenDialog(string text){
		open = true;
		if(text != ""){
			transform.FindChild("Text").GetComponent<TextMesh>().text = text;
		}
		Time.timeScale = 0;
		if(transform.position.x != mainCamera.transform.position.x){
			transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z + 3);
		}
	}
	
	public void CloseDialog(){
		open = false;
		if(transform.position.x == mainCamera.transform.position.x){
			Time.timeScale = 1;
			transform.position = new Vector3(mainCamera.transform.position.x + 1000, mainCamera.transform.position.y + 1000, mainCamera.transform.position.z - 20);
		}
	}
}
