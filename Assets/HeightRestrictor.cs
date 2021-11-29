using UnityEngine;
using System.Collections;

public class HeightRestrictor : MonoBehaviour {

	private float height = 0f;
	private Rigidbody rg;
	private static Vector3 center = new Vector3(0,0,0);
	// Use this for initialization
	void Start () {
		rg = this.gameObject.GetComponent<Rigidbody> ();
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 position = this.gameObject.transform.position;
		position.z = height;
		this.gameObject.transform.position = position;
		//float forceZ = rg.GetRelativePointVelocity (center);
		Vector3 vel = rg.velocity;
		vel.z = 0;
		rg.velocity = vel;
	}
}
