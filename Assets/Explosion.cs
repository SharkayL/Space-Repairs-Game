using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Explosion : PooledObj {
	
	private BulletPool poolHandler;
	public float lifeTime=0.5f;
	private float expiery=0;
	private float scale=1f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(expiery<0){
			this.ReturnToPool();
		}
		expiery-=Time.deltaTime;
		float ratio = (expiery/lifeTime) * scale;
		this.audio.volume=(expiery/lifeTime) * scale;
		this.transform.localScale = new Vector3(ratio,ratio,ratio);
	}

	public void reset(float scale){
		this.scale=scale;
		expiery=lifeTime;
		this.transform.localScale.Set(scale,scale,scale);
		this.audio.Play();
	}
	
}
