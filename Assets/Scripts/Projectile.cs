using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {
	
	private Transform[] heavenlyBodies;
	private Transform heavenlyBodyParent;
	public float initialVelocity = 2F;
	public float constantVelocity = 0;
	public float gravitationalConstant = 3F;
	public float damage = 5;
	public float originalXScale;
	
	// Use this for initialization
	void Start () {
		transform.parent = GameObject.Find ("Projectiles").transform;
		heavenlyBodyParent = GameObject.Find("HeavenlyBodies").transform;
		heavenlyBodies = new Transform[heavenlyBodyParent.childCount];
		for(int i = 0; i < heavenlyBodyParent.childCount; i++){
			heavenlyBodies[i] = heavenlyBodyParent.GetChild(i);
		}
		rigidbody.velocity = transform.forward * initialVelocity;
		damage *= transform.localScale.x / originalXScale;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(constantVelocity != 0){
			rigidbody.AddForce(transform.forward * constantVelocity);
		}
		for(int i = 0; i < heavenlyBodyParent.childCount; i++){
			float force = -(heavenlyBodies[i].localScale.x * gravitationalConstant / Vector3.SqrMagnitude(new Vector3(transform.position.x - heavenlyBodies[i].position.x, transform.position.y - heavenlyBodies[i].position.y, 0)));
			Vector3 direction = Vector3.Normalize(new Vector3(transform.position.x - heavenlyBodies[i].position.x, transform.position.y - heavenlyBodies[i].position.y, 0));
			rigidbody.AddForce(force * direction);
		}
	}
	
	void OnCollisionEnter(Collision collision) {
		Destroy(gameObject);
	}
}
