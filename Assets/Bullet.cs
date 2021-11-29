using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bullet : PooledObj {

	public float damage;
	private float lifetime;
	private float maxLifetime=5f;
	//private Renderer validRender;
	// Use this for initialization
	void Start () {
		//validRender = this.transform.FindChild("Cube").renderer;
	}

	private void OnCollisionEnter(Collision collision){
		if(!this.gameObject.activeSelf){
			//Debug.Log ("Stupid");
			return;
		}

		this.ReturnToPool();
		GameObject other = collision.gameObject;
		//if(validRender.isVisible){
			BulletPool.activePool.Explode(this.transform.position,2f);
		//}
		lifetime=0f;
		if(other.GetComponent<DamagebleObj>()!=null){
			other.GetComponent<DamagebleObj>().DamageBy(10);
		}
	}


	
	// Update is called once per frame
	void Update () {
		lifetime+=Time.deltaTime;
		if(lifetime>maxLifetime){
			this.ReturnToPool();
			lifetime=0f;
		}
	}

}
