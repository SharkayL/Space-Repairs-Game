using UnityEngine;
using System.Collections;

public class TeleObj : PooledObj {

	private AnimationState anim;
	private Renderer rend;

	// Use this for initialization
	void Start () {
		anim=animation["Action"];
		animation.clip=anim.clip;
		animation.Play();
		//anim.speed=0.1f;
		rend=transform.FindChild("Main").renderer;

		AnimationEvent animEvent = new AnimationEvent();
		animEvent.time=anim.length;
		animEvent.functionName="Hide";

		anim.clip.AddEvent(animEvent);
		audio.Play();

	}


	void Hide(){
		//Debug.Log ("Gone!");
		this.ReturnToPool();
	}

	// Update is called once per frame
	void Update () {

		float prog = animation["Action"].normalizedTime;
		rend.material.color = new Color(0,1,1,1f-prog);

	}
}
