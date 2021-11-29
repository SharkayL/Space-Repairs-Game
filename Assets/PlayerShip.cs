using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class BurnerControll {
	private ParticleSystem particles;
	private int count;

	public BurnerControll(GameObject obj,int partCount){
		particles = obj.GetComponents<ParticleSystem> ()[0];
		count = partCount;
	}

	public void activate(){
		particles.enableEmission = true;
	}

	public void thrust(float mul){
		particles.maxParticles = (int)(count * mul);
	}

	public void deactivate(){
		particles.enableEmission = false;
	}


}

public class PlayerShip : MonoBehaviour,IDamage {

	private Vector3 up = new Vector3(0f,0,-1f);
	private Vector3 center = new Vector3(0,0,0);
	private float fixedHeight = 0f;
	private Vector3 forward;
	private float stabiliser = 150f*100f;
	private float sideStabiliser = 0.1f*100f;
	private float angStabiliser = 50f*200f;
	private float acceleration = 80f*100f;
	private float angAcc = 30f*100f;
	private Vector3 velocity = new Vector3 (0f, 0f, 0f);
	private Vector3 appliedForce = new Vector3 (0f, 0f, 0f);
	private Rigidbody rg;
	private float velCap = 30f;
	private float angVel = 0f;
	private float maxForce = 200f*100f;
	private Vector3 objectScanOffset = new Vector3(2,0,0);
	private float scanLength = 5;
	private int rays = 6;
	private float fanAngle = 90f;
	private float fanAngleSection;
	private Throwable grabedObject = default(Throwable);
	private FixedJoint throwableJoint;
	private float throwTimeoutCounter = 0;
	private float throwTimeout = 0.5f;
	private ParticleSystem burst;
	private ParticleSystem repairStream;
	private AudioSource repairSound;
	private ParticleSystem selfRepair;

	//private Text hitCounter;
	private BurnerControll leftBack;
	private BurnerControll rightBack;

	private GameObject pointer;
	public static GameObject player;

	private int hits = 0;
	private int maxHits = 100;
//	private static Vector3[] = {
	
	private int lifeTime = 200;
	// Use this for initialization
	void Start () {
		burst = this.transform.FindChild("Eject").GetComponent<ParticleSystem>();
		repairStream = this.transform.FindChild("Repair").GetComponent<ParticleSystem>();
		selfRepair = this.transform.FindChild("Cabin").GetComponent<ParticleSystem>();
		repairSound = this.transform.FindChild("Repair").GetComponent<AudioSource>();
		audio.Play();
		repairStream.enableEmission=false;
		PlayerShip.player=this.gameObject;
		this.GetComponent<DamagebleObj>().RegisterReciver(this);
		pointer = Instantiate(Resources.Load<GameObject>("WrenchOBJ")as GameObject) as GameObject;
		pointer.SetActive(true);

		this.fanAngleSection = fanAngle / rays;
		//hitCounter = GameObject.FindWithTag("HitCounter").GetComponent<Text>();
		Vector3 right = Vector3.Cross (this.transform.forward, up);
		forward = Vector3.Cross (right, up);
		rg = this.GetComponent<Rigidbody> ();
		forward.Normalize ();
		this.transform.transform.rotation = this.makeOrientation (up, forward);

		leftBack = new BurnerControll(this.transform.FindChild("Left Back Thruster").gameObject,800);
		rightBack = new BurnerControll(this.transform.FindChild("Right Back Thruster").gameObject,800);
		//leftBack.deactivate();
		//rightBack.deactivate ();
	}
	
	// Update is called once per frame
	void Update () {
		forward = this.transform.forward;
		forward = Vector3.Cross (up, forward);
		forward = Vector3.Cross (forward, up);
		forward.Normalize();

		bool forwardKey = Input.GetKey (KeyCode.W);
		bool backKey = Input.GetKey (KeyCode.S);
		velocity = rg.GetRelativePointVelocity (center);
		appliedForce.Set (0, 0, 0);

		if (forwardKey && !backKey) {
			leftBack.activate ();
			rightBack.activate ();
			audio.volume=1f;
			if (Vector3.Dot (forward, velocity) < 0) {
				appliedForce -= forward * (acceleration + stabiliser);

			} else {
				appliedForce -= forward * acceleration;
			}
			//		velocity = velocity+force;
		} else {
			leftBack.deactivate();
			rightBack.deactivate();
			audio.volume=0f;
			if (backKey && !forwardKey) {
				appliedForce += forward * acceleration;
				//		velocity = velocity-force;
			} else {
				if (velocity.magnitude != 0) {
					if (velocity.magnitude < stabiliser) {
						appliedForce -= velocity;
					} else {
						appliedForce.Set (-velocity.x, -velocity.y, -velocity.z);
						appliedForce.Normalize ();
						appliedForce *= stabiliser;

					}
				}
			}
		}
		bool leftKey = Input.GetKey (KeyCode.A);
		bool rightKey = Input.GetKey (KeyCode.D);

		angVel = rg.angularVelocity.z*60*100;
		float angForce = 0f;
		if (leftKey && !rightKey) {
			if(angVel>0){
				angForce-=angStabiliser;
			}
			angForce-=angAcc;
		} else if (rightKey && !leftKey) {
			if(angVel<0){
				angForce+=angStabiliser;
			}
			angForce+=angAcc;
		} else {
			if(Mathf.Abs (angForce)<angStabiliser){
				angForce=angVel;//Booooo, why you FPS dependant
			}else{
				if(angForce<0){
					angForce+=angStabiliser;
				}else{
					angForce-=angStabiliser;
				}
			}
		}

		float forwardDot = 1f-Vector2.Dot (appliedForce, forward);
		if (forwardDot > 0) {

		} else {

		}
		rg.AddTorque (up * angForce);

		rg.AddForce (appliedForce);

		if (rg.velocity.magnitude > velCap) {
			rg.velocity=rg.velocity.normalized * velCap;
		}
		//velocity += appliedForce;

		//if(velocity.magnitude>velCap){
		//	velocity.Normalize();
		//	velocity = velocity * velCap;
		//}

		//Vector3 position = this.transform.position;
		//position.x = position.x + velocity.x;
		//position.y = position.y + velocity.y;
		//this.transform.position = position;
		//this.transform.RotateAround (position, up, angVel);
		Vector3 position = this.gameObject.transform.position;

		Vector3 depoPos = DepoObj.active.transform.position;

		selfRepair.enableEmission=false;
		if((position-depoPos).magnitude<3){

			if(hits>0){
				hits-=1;
				selfRepair.enableEmission=true;
			}

		}

		Vector3 rayOrigin;
		position.z = fixedHeight;
		this.gameObject.transform.position = position;

		this.gameObject.transform.rotation = Quaternion.LookRotation (forward, up);
		Quaternion rayOrientation = Quaternion.LookRotation (up, Vector3.Cross(forward,up));
		Vector3 direction;
		RaycastHit hit;
		if(grabedObject==null){

			rayOrigin = position + rayOrientation * objectScanOffset;
			direction = new Vector3(1,0,0);

			Vector3 fixDirection = rayOrientation * direction;
			Physics.Raycast(position,fixDirection,out hit,4);
			repairStream.enableEmission=false;

			if(hit.rigidbody!=null){
				FixableObj fixedObj = hit.rigidbody.gameObject.GetComponent<FixableObj>();
				if(fixedObj!=null){
					fixedObj.FixBy(2);
					repairStream.enableEmission=true;
					if(!repairSound.isPlaying){
						repairSound.Play();
					}
				}
			}
			else{
				if(repairSound.isPlaying){
					repairSound.Stop();
				}
			}

			for(int i=0;i<(rays+1);i++){
				Vector2 rayDirection = new Vector2(1,0);
				float angle = fanAngleSection * i - fanAngle / 2;
				rayDirection=rayDirection.Rotate(angle);
				direction.x=rayDirection.x;
				direction.y=rayDirection.y;
				direction.z=0;
				direction = rayOrientation * direction;

				//Debug.DrawLine(rayOrigin,rayOrigin+direction);


				if(Physics.Raycast(rayOrigin,direction,out hit,scanLength,1)){
					if(hit.rigidbody!=null){
						//hit.rigidbody.AddForce(direction * 10000);
						GameObject obj = hit.rigidbody.gameObject;
						Throwable throwObj = obj.GetComponent<Throwable>();
						if(throwObj!=null){
							grabedObject = throwObj;
							grabedObject.Reset();
							throwableJoint = obj.AddComponent<FixedJoint>();
							throwableJoint.enableCollision=false;
							grabedObject.transform.position=rayOrigin + rayOrientation * new Vector3(1,0,0);

							throwableJoint.connectedBody = this.rigidbody;

							throwableJoint.breakForce = 200;
							throwTimeoutCounter=throwTimeout*2f;
							break;
						}
					}
				}
			}
		}
		else{
			if(throwableJoint!=null&&grabedObject.gameObject.activeSelf){
				if(Input.GetKey(KeyCode.Space)){
					Destroy(throwableJoint);
					burst.Play();
					Quaternion throwOrientation = Quaternion.LookRotation (up, Vector3.Cross(forward,up));
					direction = throwOrientation * new Vector3(4000,0,0) * grabedObject.rigidbody.mass;
					grabedObject.Prime();
					grabedObject.rigidbody.velocity=new Vector3(0,0,0);
					grabedObject.rigidbody.AddForce(direction);
				}
			}
			else{
				if(throwTimeoutCounter<0){
					grabedObject = default(Throwable);
				}
				else{
					throwTimeoutCounter-=Time.deltaTime;
				}
			}
		}

		rayOrigin = position + rayOrientation * new Vector3(6,0,0);
		direction = rayOrientation * new Vector3(1,0,0);

		if(Physics.Raycast(rayOrigin,direction,out hit,10f)){
			if(!pointer.activeSelf){
	//			pointer.SetActive(true);
			}
			pointer.transform.position = hit.point - direction * 1;
			pointer.transform.rotation = Quaternion.LookRotation(new Vector3(0,0,-1),direction);
			Debug.DrawLine(rayOrigin,hit.point);
		}
		else{
			pointer.transform.position = rayOrigin + direction * 10f;
			pointer.transform.rotation = Quaternion.LookRotation(new Vector3(0,0,-1),direction);
		}

	}

	public void DamageBy(int damage){

		hits+=damage;

		if(hits>maxHits){
			ScenarioController.active.Fail();
		}
	//	Debug.Log ("HIT:"+hits);
	}

	private Quaternion makeOrientation(Vector3 up,Vector3 forward){
		//float w = Mathf.Sqrt (2f + 2f * Vector3.Dot (up, forward));
		float w = Mathf.Sqrt ((Mathf.Pow (up.magnitude,2)) * (Mathf.Pow(forward.magnitude,2)))+ Vector3.Dot(up,forward);
		Vector3 halfPart = Vector3.Cross (up, forward);
		//halfPart *= (1f / w);
		return new Quaternion (halfPart.x, halfPart.y, halfPart.z, w);
	}

	private Vector3 GetVelocity(){
		return velocity;
	}

	private Vector3 GetForce(){
		return appliedForce;
	}

	public float GetHealth(){
		return ((float)maxHits-hits)/maxHits;
	}

}
