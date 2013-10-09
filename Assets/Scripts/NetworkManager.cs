using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	private const string typeName = "NukeEmFromOrbit";
	private const string gameName = "NukeEmFromOrbit";
	public Transform player1Planet;
	public Transform player2Planet;
	private float waitTime;
	private float lastTry;
	private bool is_online = false;
	private bool is_local = false;
	private bool is_tryHosting = false;
	private bool is_looking = false;
	private int connectionAttempts = 0;
	 
	private void StartServer(){
	    Network.InitializeServer(2, 25000, !Network.HavePublicAddress());
	    MasterServer.RegisterHost(typeName, gameName);
	}
	
	void OnServerInitialized(){
	    Debug.Log("Server Initializied");
		player2Planet.GetComponent<PlanetaryControls>().Remote("server");
	}
	
	void Disconnect () {
		Network.Disconnect();
		is_online = false;
		connectionAttempts = 0;
	}
	
	// Use this for initialization
	void Start () {
		lastTry = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		if(is_online && Network.connections.Length == 0){
			if(waitTime + lastTry < Time.time){
				if(connectionAttempts > 3){
					Disconnect();
				}
				if(is_tryHosting){
					lastTry = Time.time + 5;
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
	}
	
	void OnGUI()
	{
	    if (!is_online && !is_local){
			is_looking = false;
	        if (GUI.Button(new Rect(100, 100, 250, 100), "Play Online")){
	            is_online = true;
				is_looking = true;
			}
	        if (GUI.Button(new Rect(100, 250, 250, 100), "Play Local")){
				is_local = true;
			}
		}
		else if (is_looking){
			GUI.Box(new Rect(100,100,250,100), "Looking for Players...");
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
	}
}
