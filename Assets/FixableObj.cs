using UnityEngine;
using System.Collections;

public class FixableObj : MonoBehaviour, IFixable{

	private IFixable rec;
	// Use this for initialization
	void Start () {
		Fixing.active.Add(this);
	}
	
	// Update is called once per frame
	public void RegisterReciver(IFixable rec){
		this.rec=rec;
	}

	public void FixBy(int by){
		this.rec.FixBy(by);
	}
	
}
