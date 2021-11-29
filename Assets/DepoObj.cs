using UnityEngine;
using System.Collections;

public class DepoObj : MonoBehaviour {

	// Use this for initialization
	public static DepoObj active;

	void Awake(){
		DepoObj.active=this;
	}

	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
