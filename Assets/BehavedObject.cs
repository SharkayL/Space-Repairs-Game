using UnityEngine;
using System.Collections;

public abstract class BehavedObject : EnememyEntity {

	protected Behavior activeBehavior;

	public void ChangeBehavior(Behavior newBehavior){
		if(this.activeBehavior!=null){
			this.activeBehavior.Stop();
		}
		this.activeBehavior = newBehavior;
		if(newBehavior!=null){
			this.activeBehavior.Start(this);
		}
	}

	protected void Tick(){
		if(this.activeBehavior!=null){
			activeBehavior.Tick();
		}
	}

}
