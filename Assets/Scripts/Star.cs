using UnityEngine;
using System.Collections;

public class Star : MonoBehaviour {
	
	private bool is_growing = true;
	private float scale = 1;
	private float alpha = 1;
	public bool is_clockwise = true;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		renderer.material.color = new Color(1,1,1,alpha * (Random.value/10 - 0.05F));
		if(is_clockwise){
			transform.Rotate(0,1.5F,0);
		}
		else{
			transform.Rotate(0,-3,0);
		}
		if(is_growing){
			transform.localScale *= 1 + Random.value/10;
			scale *= 1.05F;
			if(scale > 1.3F){
				is_growing = false;
			}
		}
		else{
			transform.localScale *= 1 - Random.value/10;
			scale *= 0.95F;
			if(scale < 0.75F){
				is_growing = true;
			}
		}
	}
}
