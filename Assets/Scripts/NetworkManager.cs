﻿using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	private const string typeName = "NukeEmFromOrbit";
	private const string gameName = "NukeEmFromOrbit";
	public Transform player1Planet;
	public Transform player2Planet;
	private float waitTime = 0.5F;
	private float lastTry;
	static public bool is_online = false;
	static public bool is_local = false;
	private bool is_tryHosting = false;
	private bool is_looking = false;
	private bool is_ready = false;
	private bool is_instructions = false;
	static public bool is_menu = true;
	static public bool is_gameOver = false;
	private bool is_credits = false;
	public Font font;
	static public string winningPlayerText;
	 
	private void StartServer(){
	    Network.InitializeServer(1, 25000, !Network.HavePublicAddress());
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
			}
		}
		else if (Network.connections.Length == 1){
			is_looking = false;
			if(!is_ready){
				is_menu = true;
			}
		}
	}
	
	void OnGUI(){
		GUI.skin.font = font;
		if(is_gameOver){
			GUI.Box(new Rect(300,100,250,100), winningPlayerText);
			if (GUI.Button(new Rect(300,300,250,100), "Main Menu")){
				is_gameOver = false;
				is_online = false;
				is_local = false;
				is_menu = true;
			}
		}
		else if (is_credits){
			GUI.Box(new Rect(150,100,600,300), "Thank you for Playing!\nDesigner and Developer: David Geisert\nDesigner and Artist: Stephanie Lee\nAssisting Development: Julian Hartline\nAssisting Design: Mohamed Kazzaz\n\nMusic: Alexandr Zhelanov\nhttp://opengameart.org/content/space-1\n\nSound Effects: AstroMenace Artwork ver 1.2 \nAssets Copyright (c) 2006-2007 \nMichael Kurinnoy, Viewizard\nhttp://opengameart.org/content/space-battle-game-sounds-astromenace\n\nFor information contact dg@catduo.com\n or visit catduo.com");
			if (GUI.Button(new Rect(300,400,250,100), "Main Menu")){
				is_gameOver = false;
				is_online = false;
				is_local = false;
				is_menu = true;
				is_credits = false;
			}
		}
		else if(is_instructions){
	        if (GUI.Button(new Rect(300, 560, 250, 40), "")){
				GameObject.Find ("MainCamera").transform.position = new Vector3 (0,0,-20);
				is_instructions = false;
				is_menu = true;
				GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
			}
		}
		else if(is_menu){
			if (Network.connections.Length == 0){
			    if (!is_online && !is_local || AI.is_ai){
					GUI.Box(new Rect(300,20,250,40), "Nuke 'Em From Orbit!");
					is_looking = false;
			        if (GUI.Button(new Rect(350, 100, 150, 50), "Play Online")){
			            is_online = true;
						is_looking = true;
						AI.is_ai = false;
					}
			        if (GUI.Button(new Rect(350, 200, 150, 50), "Single Player")){
						is_local = true;
						AI.is_ai = false;
					}
			        if (GUI.Button(new Rect(350, 300, 150, 50), "Instructions")){
						GameObject.Find ("MainCamera").transform.position = new Vector3 (0,15,-20);
						is_instructions = true;
					}
			        if (GUI.Button(new Rect(350, 400, 150, 50), "Credits")){
						is_credits = true;
					}
			        if (GUI.Button(new Rect(600, 300, 100, 50), "Mute")){
						if(AudioListener.volume == 1){
							AudioListener.volume = 0;
						}
						else{
							AudioListener.volume = 1;
						}
					}
				}
				else if (is_looking){
					GUI.Box(new Rect(330, 100, 190, 50), "Looking for Players...");
					if (GUI.Button(new Rect(350, 200, 150, 50), "Stop Looking")){
						is_looking = false;
						is_online = false;
						Disconnect();
					}
			        if (GUI.Button(new Rect(350, 300, 150, 50), "Instructions")){
						GameObject.Find ("MainCamera").transform.position = new Vector3 (0,15,-20);
						is_instructions = true;
					}
			        if (GUI.Button(new Rect(350, 400, 150, 50), "Credits")){
						is_credits = true;
					}
				}
				else if(is_local){
					GUI.Box(new Rect(300,50,150,50), "Choose Difficulty");
					if (GUI.Button(new Rect(275, 100, 100, 50), "1")){
						GameObject.Find ("MainCamera").GetComponent<AI>().difficulty = 0;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
						is_menu = false;
					}
					if (GUI.Button(new Rect(275, 150, 100, 50), "2")){
						GameObject.Find ("MainCamera").GetComponent<AI>().difficulty = 1;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
						is_menu = false;
					}
					if (GUI.Button(new Rect(275, 200, 100, 50), "3")){
						GameObject.Find ("MainCamera").GetComponent<AI>().difficulty = 2;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
						is_menu = false;
					}
					if (GUI.Button(new Rect(275, 250, 100, 50), "4")){
						GameObject.Find ("MainCamera").GetComponent<AI>().difficulty = 3;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
						is_menu = false;
					}
					if (GUI.Button(new Rect(275, 300, 100, 50), "5")){
						GameObject.Find ("MainCamera").GetComponent<AI>().difficulty = 4;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
						is_menu = false;
					}
					if (GUI.Button(new Rect(375, 100, 100, 50), "6")){
						GameObject.Find ("MainCamera").GetComponent<AI>().difficulty = 5;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
						is_menu = false;
					}
					if (GUI.Button(new Rect(375, 150, 100, 50), "7")){
						GameObject.Find ("MainCamera").GetComponent<AI>().difficulty = 6;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
						is_menu = false;
					}
					if (GUI.Button(new Rect(375, 200, 100, 50), "8")){
						GameObject.Find ("MainCamera").GetComponent<AI>().difficulty = 7;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
						is_menu = false;
					}
					if (GUI.Button(new Rect(375, 250, 100, 50), "9")){
						GameObject.Find ("MainCamera").GetComponent<AI>().difficulty = 8;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
						is_menu = false;
					}
					if (GUI.Button(new Rect(375, 300, 100, 50), "10")){
						GameObject.Find ("MainCamera").GetComponent<AI>().difficulty = 9;
						GameObject.Find ("NewGame").GetComponent<NewGame>().Tap();
						GameObject.Find ("MainCamera").GetComponent<AI>().Setup();
						is_menu = false;
					}
				}
			    if (GUI.Button(new Rect(30, 275, 100, 50), "Menu")){
					if(is_menu){
						is_menu = false;
					}
					else{
						is_menu = true;
					}
				}
			}
			else if(!is_ready && !is_looking && is_online){
				if (GUI.Button(new Rect(300, 250, 250, 100), "Ready!")){
					is_ready = true;
					NewGame.readyCount++;
					GameObject.Find ("Player1Planet").GetComponent<PlanetaryControls>().Ready();
				}
			}
			else if (NewGame.readyCount == 2 && !is_looking && is_online){
				GUI.Box(new Rect(300,100,250,100), "Waiting for other Players...");
			}
			else{
				is_menu = false;
			}
		    if (GUI.Button(new Rect(30, 275, 100, 50), "Menu")){
				if(is_menu){
					is_menu = false;
				}
				else{
					is_menu = true;
				}
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
		is_menu = true;
	}
}
