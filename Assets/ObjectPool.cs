using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool{

	private List<PooledObj> returnPool;
	private List<PooledObj> deleteQue;
	private LinkedList<PooledObj> activePool;

	public ObjectPool(){
		this.returnPool=new List<PooledObj>(30);
		this.deleteQue=new List<PooledObj>(30);
		this.activePool=new LinkedList<PooledObj>();
	}

	public ObjectPool(int size){
		this.returnPool=new List<PooledObj>(size);
		this.deleteQue=new List<PooledObj>(size);
		this.activePool=new LinkedList<PooledObj>();
	}

	public void ReturnToPool(PooledObj obj,LinkedListNode<PooledObj> node){
		deleteQue.Add (obj);
		//activePool.Remove (node);
	}

	public void Update(){
		while(deleteQue.Count!=0){
			PooledObj obj = deleteQue[0];
			deleteQue.RemoveAt(0);
			if(obj.GetPoolNode()==null){
				Debug.Log ("Faulty return!");
			}
			activePool.Remove (obj.GetPoolNode());
			obj.SetPoolNode(default(LinkedListNode<PooledObj>));
			returnPool.Add(obj);
		}
	}

	public void Add(PooledObj obj){
		obj.SetPool(this);
		this.returnPool.Add (obj);
	}

	public PooledObj Get(){
		int index = returnPool.Count-1;
		if(index==-1){
			return default(PooledObj);
		}

		PooledObj obj = returnPool[index];
		returnPool.RemoveAt(index);
		LinkedListNode<PooledObj> node = activePool.AddLast(obj);
		obj.SetPoolNode(node);
		return obj;
	}

	public void ReturnAll(){
		while(activePool.First!=null){
			PooledObj obj = activePool.First.Value;
			activePool.RemoveFirst();
			obj.SetPoolNode(default(LinkedListNode<PooledObj>));
			returnPool.Add(obj);
			obj.gameObject.SetActive(false);
		}
	}

	public bool Available(){
		return returnPool.Count!=0;
	}

	public LinkedList<PooledObj> GetActive(){
		return activePool;
	}

	
}
