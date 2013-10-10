﻿using UnityEngine;
using System.Collections;

public class AI : MonoBehaviour {
	
	private Transform aiPlanet;
	private Transform up;
	private Transform down;
	private Transform left;
	private Transform right;
	private Transform selected;
	private Transform[] planetaryPositions;
	
	private Transform towardsPlayer;
	private float towardsPlayerSwitchTime;
	private int aiComputeLag = 0;
	private int money;
	private int[] costs = new int[4];
	private int difficulty = 1;
	
	// Use this for initialization
	public void Setup () {
		aiPlanet = GameObject.Find ("Player2Planet").transform;
		up = aiPlanet.FindChild("Up");
		down = aiPlanet.FindChild("Down");
		left = aiPlanet.FindChild("Left");
		right = aiPlanet.FindChild("Right");
		planetaryPositions = new Transform[] {left, down, right, up};
		towardsPlayer = planetaryPositions[3];
		selected = up;
		SetBuildings();
		GetCosts();
		difficulty++;
	}
	
	// Update is called once per frame
	void Update () {
		if(NetworkManager.is_local){
			if(aiComputeLag >= 5){
				aiComputeLag = 0;
				Debug.Log ("AI computing");
				money = aiPlanet.GetComponent<PlanetaryControls>().playerMoney;
				for(int i = 0; i < 4; i++){
					if(money >= costs[i]){
						planetaryPositions[i].GetComponent<Building>().Construct();
						GetCosts();
					}
					if(planetaryPositions[i].GetComponent<Building>().is_buildingReady){
						planetaryPositions[i].GetComponent<Building>().Action();
					}
				}
			}
			else{
				aiComputeLag++;
			}
		}
	}
	
	void Select (Transform selection){
		selected.GetComponent<Building>().UnSelected();
		selected = selection;
		selected.GetComponent<Building>().Selected();
	}
	void Scroll (Transform selection, int count){
		for(int i = 0; i < count; i++){
			selection.GetComponent<Building>().Scroll(1);
		}
	}
	void GetCosts (){
		for(int i = 0; i < 4; i++){
			costs [i] = planetaryPositions[i].GetComponent<Building>().cost;
		}
	}
	void SetBuildings () {
		switch(difficulty){
		case 0:
			Scroll(left, 0);
			Scroll(down, 0);
			Scroll(right, 0);
			Scroll(up, 0);
			break;
		case 1:
			Scroll(left, 1);
			Scroll(down, 2);
			Scroll(right, 3);
			Scroll(up, 4);
			break;
		default:
			break;
		}
	}
}
