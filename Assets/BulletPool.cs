using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour {

	public int maxBigBullets = 30;
	public int maxMicroBullets = 100;
	public int maxExplosions = 20;
	public int maxBarrels = 20;
	public int maxTeles = 10;
	public bool pregenerate = true;
	public string bigBullet = "Bullet";
	public string microBullet = "Micro Bullet";
	public string explosion = "Explosion";
	private GameObject bigBulletObj;
	private GameObject microBulletObj;
	private GameObject explosionObj;
	public GameObject barrelObj;
	private GameObject teleObj;
	private ObjectPool bullets;
	private ObjectPool explosions;
	public ObjectPool barrels;
	public ObjectPool teles;

	private Vector3 up = new Vector3(0,0,-1f);

	public static BulletPool activePool;
	// Use this for initialization
	void Start () {
		BulletPool.activePool=this;

		bullets = new ObjectPool(maxBigBullets);
		explosions = new ObjectPool(maxExplosions);
		barrels = new ObjectPool(maxBarrels);
		teles = new ObjectPool(maxTeles);

		bigBulletObj = Resources.Load("Bullet") as GameObject;
		explosionObj = Resources.Load("Explosion") as GameObject;
		teleObj = Resources.Load ("Tele") as GameObject;

		for(int i=0;i<maxBigBullets;i++){
			GameObject obj = (GameObject)Instantiate(bigBulletObj,new Vector3(0,0,0),Quaternion.identity);
			obj.SetActive(false);
			bullets.Add(obj.GetComponent<Bullet>());
		}

		for(int i=0;i<maxExplosions;i++){
			GameObject obj = (GameObject)Instantiate(explosionObj,new Vector3(0,0,0),Quaternion.identity);
			obj.SetActive(false);
			explosions.Add(obj.GetComponent<Explosion>());
		}

		for(int i=0;i<maxBarrels;i++){
			GameObject obj = (GameObject)Instantiate(barrelObj,new Vector3(0,0,0),Quaternion.identity);
			obj.SetActive(false);
			barrels.Add(obj.GetComponent<Throwable>());
		}

		for(int i=0;i<maxTeles;i++){
			GameObject obj = (GameObject)Instantiate(teleObj);
			obj.SetActive(false);
			teles.Add(obj.GetComponent<TeleObj>());
		}
	}

	public GameObject Launch(GameObject originator,Vector3 origin,Vector3 direction,float speed){
		if(!bullets.Available()){
			return default(GameObject);
		}
		direction=direction.normalized;
		GameObject currentBullet = bullets.Get().gameObject;
		currentBullet.transform.position=origin;

		currentBullet.transform.rotation=Quaternion.LookRotation(direction,up);
		currentBullet.SetActive(true);
		Rigidbody currentRG = currentBullet.GetComponent<Rigidbody>();

		currentRG.velocity=direction*speed;
		currentRG.angularVelocity=new Vector3(0,0,0);
		Physics.IgnoreCollision(currentRG.collider,originator.GetComponent<Rigidbody>().collider);

		return currentBullet;
	}

	public GameObject Explode(Vector3 position,float scale){
		if(!explosions.Available()){
			return default(GameObject);
		}
		GameObject currentExplosion = explosions.Get().gameObject;
		currentExplosion.transform.position=position;
		currentExplosion.SetActive(true);
		Explosion explode = currentExplosion.GetComponent<Explosion>();
		explode.reset(scale);
		return currentExplosion;
	}

	public GameObject DispenseBarrel(Vector3 position){

		if(!barrels.Available()){
			return default(GameObject);
		}
		GameObject currentBarrel = barrels.Get().gameObject;
		currentBarrel.transform.position=position;
		currentBarrel.transform.rotation=Quaternion.identity;
		currentBarrel.SetActive(true);
		return currentBarrel;
	}

	public GameObject Tele(Vector3 position){
		if(!teles.Available()){
			return default(GameObject);
		}
		GameObject obj = teles.Get().gameObject;
		obj.transform.position=position;
		obj.SetActive(true);
		return obj;
	}

	public void ReturnBigBullet(GameObject bullet){

	}

	// Update is called once per frame
	void Update () {
	
	}

	void LateUpdate(){
		bullets.Update();
		explosions.Update();
		barrels.Update ();
	}
}
