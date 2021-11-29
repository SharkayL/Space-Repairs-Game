using UnityEngine;
using System.Collections;

public class Particle : MonoBehaviour {

	private static Vector3 depth = new Vector3 (0, 0, 1);

	private int lifeTime = 200;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (lifeTime == 0) {
			//Destroy(this);
			Destroy(this.gameObject);
			return;
		}
		float scale = (lifeTime / 200f)*0.5f;
		//this.transform.localScale.Set(scale,scale,scale);
		this.gameObject.transform.localScale = new Vector3 (scale, scale, scale);
		this.gameObject.transform.Translate (depth);
		//this.transform.
		lifeTime--;
	}
}
