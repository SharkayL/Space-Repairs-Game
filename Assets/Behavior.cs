using UnityEngine;
using System.Collections;

public class Behavior {

	protected BehavedObject gameObject;

	public virtual void Start(BehavedObject gameObject){
		this.gameObject=gameObject;
	}

	public virtual void Stop(){

	}

	public virtual void Tick(){

	}

}
