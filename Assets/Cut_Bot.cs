using UnityEngine;
using System.Collections;

public class CutBotBehaviour : Behavior{

	private AnimationState walk;
	private float maxVel = 30f;
	private float force = 400f;
	private float animSpeed = 1f;
	private Vector3 up = new Vector3(0f,0f,-1f);
	private Transform tower;
	private Vector3 towerTarget =new Vector3(1f,0,0);
//	private float towerMaxSpinRate = 0.15f;

	public override void Start(BehavedObject gameObject){
		base.Start(gameObject);
		walk=gameObject.gameObject.animation["Walk"];
		tower=gameObject.gameObject.transform.FindChild("Head").transform;
	}

	public override void Stop(){
		gameObject.gameObject.animation.Stop();
		//this.gameObject.audio.Stop();
		EnemyManager.activeManager.RemoveEnemy((EnememyEntity) gameObject);
	}

	public override void Tick(){
		Rigidbody rg = this.gameObject.rigidbody;
		Vector3 velocity = rg.velocity;
		walk.speed=velocity.magnitude/animSpeed;
		
		Vector3 target=Vector3.Cross(up,velocity);
		
		this.gameObject.transform.rotation = Quaternion.LookRotation(up,-target);
		Vector3 position = this.gameObject.transform.position;
		position.z=0f;
		this.gameObject.transform.position=position;
		
		
		Vector3 playerPos = PlayerShip.player.transform.position;
		Vector3 targetPosition;
		if((playerPos-position).magnitude<40){
			targetPosition = playerPos;
		}
		else{
			GameObject chosenObject = PlayerShip.player.gameObject;
			float dist=99999f;
			foreach(GameObject potentialTarget in EnemyManager.activeManager.targets){
				float potDist = (potentialTarget.transform.position-position).magnitude;
				if(potDist<dist){
					chosenObject = potentialTarget;
					dist = potDist;
				}
			}
			targetPosition=chosenObject.transform.position;
		}
		
		targetPosition = (targetPosition-position);
		Vector3 targetOffset = -Vector3.Cross(targetPosition.normalized,up);
		Vector3 tempPosition = targetPosition-targetOffset;
		Vector3 targetForce = tempPosition.normalized * maxVel - rg.velocity;
		targetForce = targetForce * force;
		
		rg.AddForce(targetForce);

		Vector3 rayDirection = this.gameObject.transform.rotation*new Vector3(-1.5f,0,0);
		Vector3 rayOrigin = this.gameObject.transform.position;

		RaycastHit hit;

		if(Physics.Raycast(rayOrigin,rayDirection,out hit,3)){
			Rigidbody objRg = hit.collider.rigidbody;
			if(objRg!=null){
				DamagebleObj obj = objRg.gameObject.GetComponent<DamagebleObj>();
				if(obj!=null){
					obj.DamageBy(1);
				}
			}
		}

		Debug.DrawLine(this.gameObject.transform.position,rayOrigin);
	}
}

public class Cut_Bot : BehavedObject {

	public float speed = 20f;
	public int health = 2;
	public float disolveTime = 2f;
	// Use this for initialization
	void Start () {
		EnemyManager.activeManager.AddEnemy(this);
		AnimationState cut = animation["Cut"];
		AnimationState walk = animation["Walk"];
		animation.clip=walk.clip;
		animation.Play();
		Transform topNode = transform.FindChild("Head");
		cut.AddMixingTransform(topNode);

		cut.blendMode=AnimationBlendMode.Blend;
		cut.layer=10;
		cut.weight=1f;
		cut.enabled=true;

		this.ChangeBehavior(new CutBotBehaviour());
	}
	
	// Update is called once per frame
	void Update () {
		this.Tick();
	}

	public override void Damage(int damage){
		this.health-=1;
		//Debug.Log ("Damaged");
		if(health==0){
			this.ChangeBehavior(new Dead(this));
		}
	}
	
	private class Dead : Behavior{
		
		private Cut_Bot bot;
		public Dead(Cut_Bot bot){
			this.bot=bot;
		}
		
		public override void Tick ()
		{
			bot.disolveTime-=Time.deltaTime;
			if(bot.disolveTime<0){
				bot.gameObject.SetActive(false);
				//EnemyManager.activeManager.RemoveEnemy(bot);
				BulletPool.activePool.Explode(bot.transform.position,3f);
			}
		}
	}
}
