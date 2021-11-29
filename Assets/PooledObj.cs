using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PooledObj : MonoBehaviour {

	protected ObjectPool pool;
	protected LinkedListNode<PooledObj> node;

	public void SetPool(ObjectPool pool){
		this.pool=pool;
	}

	public void SetPoolNode(LinkedListNode<PooledObj> node){
		this.node=node;
	}

	public LinkedListNode<PooledObj> GetPoolNode(){
		return node;
	}
	
	public void ReturnToPool(){
		if(node!=null){
			pool.ReturnToPool(this,node);
			this.gameObject.SetActive(false);
		}
		else{
			Debug.Log ("Faulty pool return regisrted!");
		}
	}
	
}
