using UnityEngine;
using System.Collections;

public class Dispenser : MonoBehaviour {

	private float reload=0f;
	private float reloadTime = 5f;
	private float dispensingAcc = 0;
	private float dispensingTime = 0.5f;
	private float restartDist = 2f;
	private bool busy = false;
	private GameObject dispensedBarrel;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		if(busy){
			dispensingAcc+=Time.deltaTime;
			Vector3 currentPos = this.gameObject.transform.position;
			currentPos.z=currentPos.z+3-4*(dispensingAcc/dispensingTime);
			dispensedBarrel.transform.position=currentPos;
			if(dispensingAcc>dispensingTime){
				dispensedBarrel.rigidbody.isKinematic=false;
				dispensedBarrel.GetComponent<HeightRestrictor>().enabled=true;
				busy=false;
			}
		}
		else{
			if(reload<0){
				reload=reloadTime;
				Vector3 startPos = this.gameObject.transform.position;
				startPos.z = startPos.z+3;
				dispensedBarrel = BulletPool.activePool.DispenseBarrel(startPos);
				if(dispensedBarrel==null){
					return;
				}
				this.animation.Play();
				this.busy=true;
				this.dispensingAcc=0;


				dispensedBarrel.GetComponent<Throwable>().watched=true;
				dispensedBarrel.rigidbody.isKinematic=true;
				dispensedBarrel.GetComponent<HeightRestrictor>().enabled=false;
			}
			else{
				if(dispensedBarrel==null){
				reload-=Time.deltaTime;
				}
				else{
					float dist = (dispensedBarrel.transform.position-this.transform.position).magnitude;
					if(dist>restartDist||!dispensedBarrel.activeSelf){
						dispensedBarrel.GetComponent<Throwable>().watched=false;
						dispensedBarrel=default(GameObject);
					}
				}
			}
		}
	}
}
