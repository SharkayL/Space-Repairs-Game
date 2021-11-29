using UnityEngine;
using System.Collections;

public class PlayerFollow : MonoBehaviour {

	GameObject target;
	public float above = 20f;
	private static GameObject player;
	// Use this for initialization
	void Start () {
		if (player == null) {
			target = GameObject.FindWithTag ("Player");
		}
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 targetVector = target.transform.position;
		Vector3 position = this.transform.transform.position;
		position.Set(targetVector.x,targetVector.y,-above);
		this.transform.position = position;
	}
}
