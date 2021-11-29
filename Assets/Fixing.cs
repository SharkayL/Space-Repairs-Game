using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Fixing : MonoBehaviour {

	// Use this for initialization
	public static Fixing active;
	private List<IFixable> fixables = new List<IFixable>();

	void Awake(){
		Fixing.active=this;
	}

	void Start () {
	
	}

	public void Add(IFixable fixable){
		fixables.Add(fixable);
	}

	// Update is called once per frame
	void Update () {
	}
}

public interface IFixable{

	void FixBy(int by);
}
