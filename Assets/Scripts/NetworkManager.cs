using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	private const string typeName = "NukeEmFromOrbit";
	private const string gameName = "NukeEmFromOrbit";
	public Transform player1Planet;
	public Transform player2Planet;
	private float waitTime = 0.5F;
	private float lastTry;
	private bool is_online = false;
	static public bool is_local = false;
	private bool is_tryHosting = false;
	private bool is_looking = false;
	private int connectionAttempts = 0;
	private bool is_ready = false;
	private bool is_instructions = false;
	static public bool is_menu = true;
	 
	private void StartServer(){
	    Network.InitializeServer(2, 25000, !Network.HavePublicAddress());
	    MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized(){
	    Debug.Log("Server Initializied");
		player2Planet.GetComponent<PlanetaryControls>().Remote("server");
		player1Planet.GetComponent<PlanetaryControls>().NotRemote("server");
	}
	
	public void Disconnect () {
		Network.Disconnect();
		is_online = false;
		connectionAttempts = 0;
		NewGame.readyCount = 0;
	}
	
	// Use this for initialization
	void Start () {
		lastTry = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(is_online && Network.connections.Length == 0){
			NewGame.readyCount = 0;
			if(waitTime + lastTry < Time.time){
				if(connectionAttempts > 8){
					Disconnect();
				}
				if(is_tryHosting){
					StartServer();
					lastTry = Time.time + 5 * waitTime;
					is_tryHosting = false;
				}
				else{
					lastTry = Time.time;
					RefreshHostList();
					if(hostList.Length > 0){
						int randomHost = Mathf.FloorToInt(Random.value * hostList.Length);
						JoinServer(hostList[randomHost]);
					}
					is_tryHosting = true;
				}
				connectionAttempts++;
			}
		}
		else if (Network.connections.Length == 1){
			is_looking = false;
		}
	}
	
	void OnGUI(){
		if(is_instructions){
	        if (GUI.Button(new Rect(240, 445, 200, 40), "")){
				GameObject.Find ("MainCamera").transform.position = new Vector3 (0,0,-20);
				is_instructions = false;
			}
		}
		else if (is_menu){
			if (Network.connections.Length == 0){
			    if (!is_online && !is_local){
					is_looking = false;
			        if (GUI.Button(new Rect(350, 100, 150, 50), "Play Online")){
			            is_online = true;
						is_looking = true;
					}
			        if (GUI.Button(new Rect(350, 200, 150, 50), "Single Player")){
						is_local = true;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
					}
			        if (GUI.Button(new Rect(350, 300, 150, 50), "Instructions")){
						GameObject.Find ("MainCamera").transform.position = new Vector3 (0,15,-20);
						is_instructions = true;
					}
				}
				else if (is_looking){
					GUI.Box(new Rect(300,100,250,100), "Looking for Players...");
				}
			}
			if(!is_ready && !is_looking && is_online){
				if (GUI.Button(new Rect(300, 250, 250, 100), "Ready!")){
					is_ready = true;
					NewGame.readyCount++;
					GameObject.Find ("Player1Planet").GetComponent<PlanetaryControls>().Ready();
				}
			}
			else if (NewGame.readyCount == 2 && !is_looking && is_online){
				GUI.Box(new Rect(300,100,250,100), "Waiting for other Players...");
			}
		}
	}
	private HostData[] hostList = new HostData[] {};
	 
	private void RefreshHostList(){
	    MasterServer.RequestHostList(typeName);
	}
	 
	void OnMasterServerEvent(MasterServerEvent msEvent){
	    if (msEvent == MasterServerEvent.HostListReceived)
	        hostList = MasterServer.PollHostList();
	}
	private void JoinServer(HostData hostData){
	    Network.Connect(hostData);
	}
	 
	void OnConnectedToServer(){
	    Debug.Log("Server Joined");
		player1Planet.GetComponent<PlanetaryControls>().Remote("client");
		player2Planet.GetComponent<PlanetaryControls>().NotRemote("client");
	}
}
