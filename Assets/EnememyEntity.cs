using UnityEngine;
using System.Collections;

public abstract class EnememyEntity : MonoBehaviour {


	public virtual void Damage(int damage){
		Debug.Log("Wrong method");
	}
}
