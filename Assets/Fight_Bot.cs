using UnityEngine;
using System.Collections;

public class FightBotBehavior : Behavior{

	private float timePassed;
	public float fireRate = 2f;
	private float targetSideOffset = 10f;
	private Vector3 towerTarget =new Vector3(1f,0,0);
	private bool fireReady;
	private float targetAngle = 0f;
	private float maxVel = 10f;
	private float force = 200f;
	private float animSpeed = 1f;
	private Transform tower;
	private AnimationState walk;
	private Vector3 up = new Vector3(0f,0f,-1f);
	private Vector3 gunOffset = new Vector3(0.8f,0f,1.3f);
	private Rigidbody playerRG;
	private Animation animator;
	private float towerMaxSpinRate = 0.05f;
	
	public override void Start(BehavedObject gameObject){
		base.Start(gameObject);
		tower = this.gameObject.transform.FindChild("Bone").FindChild("Root Node").FindChild("Tower");
		animator=this.gameObject.GetComponent<Animation>();
		animator.Play("Walk");
		this.targetSideOffset = gameObject.GetComponent<Fight_Bot>().targetSideOffset;
		
		walk=animator["Walk"];
	}

	public override void Stop(){
		animator.Stop();
		this.gameObject.audio.Stop();
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


		bool spacing=false;
		Vector3 pathPosition = default(Vector3);
		foreach(EnememyEntity otherBot in EnemyManager.activeManager.EnemeyList){
			if(otherBot==this.gameObject){
				continue;
			}

			if((otherBot.transform.position-position).magnitude<2){
				spacing=true;
				pathPosition=(otherBot.transform.position-(otherBot.transform.position-position));
				break;
			}
		}
		targetPosition = (targetPosition-position);

		if(!spacing){
			pathPosition=targetPosition;
		}
		Vector3 targetOffset = -Vector3.Cross(pathPosition.normalized,up);
		Vector3 tempPosition = pathPosition-targetOffset * targetSideOffset;
		Vector3 targetForce = tempPosition.normalized * maxVel - rg.velocity;
		targetForce = targetForce * force;
		
		rg.AddForce(targetForce);
		
		//targetPosition = player.transform.position-position;
		
		if(!fireReady){
			timePassed+=Time.deltaTime;
			if(timePassed>fireRate){
				timePassed=0f;
				fireReady=true;
			}
		}
		else{
			float dist = targetPosition.magnitude;
			if(Vector3.Dot(targetPosition.normalized,towerTarget)>0.95&&dist<60){
				fireReady=false;
				BulletPool.activePool.Launch(this.gameObject.gameObject,tower.transform.position,towerTarget,40f);
				
			}
		}
		
		Vector3 cross = Vector3.Cross(targetPosition.normalized,towerTarget);
		cross = Vector3.Cross(towerTarget,cross).normalized;
		towerTarget=(towerTarget+cross*towerMaxSpinRate).normalized;
		//print (towerTarget);
		towerTarget.z=0f;
		tower.rotation=Quaternion.LookRotation(up,Vector3.Cross(towerTarget,up));
	}
}

public class Fight_Bot : BehavedObject {



	private PlayerManager manager;

	private Rigidbody rg;
	private float mass;

	public float targetSideOffset = 10f;

	public int health = 2;

	public float disolveTime = 2f;
	// Use this for initialization
	void Start () {
		GameObject controller = GameObject.FindWithTag("GameController");
		controller.GetComponent<EnemyManager>().AddEnemy(this);
		rg=this.GetComponent<Rigidbody>();
		mass = rg.mass;

		float whoCares = Time.deltaTime;
		this.audio.Play();
		this.ChangeBehavior(new FightBotBehavior());

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

		private Fight_Bot bot;
		public Dead(Fight_Bot bot){
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
