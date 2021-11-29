using UnityEngine;
using System.Collections;

public class Throwable : PooledObj {
	
	public float explodeDistance = 1f;
	public float blastRadius = 2f;
	public float lifeTime = 15f;
	private float curTime = 0;
	public bool primed = false;
	public bool watched = true;
	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
		if(primed){
			bool explode=false;
			Vector3 currentPosition = transform.position;
			foreach(EnememyEntity enemy in EnemyManager.activeManager.EnemeyList){
				float distance = (currentPosition-enemy.transform.position).magnitude;
				if(distance<explodeDistance){
					explode=true;
				}
			}
			if(explode){
				foreach(EnememyEntity enemy in EnemyManager.activeManager.EnemeyList){
					float distance = (currentPosition-enemy.transform.position).magnitude;
					if(distance<blastRadius){
						enemy.Damage(1);
					}
				}
				BulletPool.activePool.Explode(currentPosition,5f);
				this.ReturnToPool();
				this.Reset();
			}
		}
		if(!watched){
			curTime+=Time.deltaTime;
			if(curTime>lifeTime){
				BulletPool.activePool.Explode(transform.position,1f);
				this.ReturnToPool();
				this.Reset();
			}
		}
	}

	public void Prime(){
		curTime=0;
		this.primed=true;
	}

	public void Reset(){
		curTime=0;
		this.primed=false;
		this.watched=false;
	}
}
