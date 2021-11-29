using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyQueSpot{
	public float time;
	public GameObject unin;
	public Vector3 position;

	public EnemyQueSpot(float time,GameObject unin,Vector3 pos){
		this.time=time;
		this.unin=unin;
		this.position=pos;
	}

}

public class EnemyQue{
	private LinkedList<EnemyQueSpot> queue = new LinkedList<EnemyQueSpot>();
	private float timer;

	public void Add(float time,GameObject unin,Vector3 position){
		queue.AddLast(new EnemyQueSpot(time,unin,position));
	}

	public void Tick(){
		if(queue.First!=null){
			LinkedListNode<EnemyQueSpot> node = queue.First;
			EnemyQueSpot spot = node.Value;

			timer+=Time.deltaTime;
			if(timer>spot.time){
				timer=0;
				queue.Remove(node);
				if(spot.position!=null){
					Vector3 position = spot.position;
					GameObject obj = MonoBehaviour.Instantiate(spot.unin,position,Quaternion.identity) as GameObject;
					BulletPool.activePool.Tele(position);
					obj.SetActive(true);
                }
				else{
					Vector3 position = ArenaProvider.active.GetRandomTile().transform.position;
					GameObject obj = MonoBehaviour.Instantiate(spot.unin,position,Quaternion.identity) as GameObject;
					BulletPool.activePool.Tele(position);
					obj.SetActive(true);
				}
			}

		}
	}

	public void Force(){
		LinkedListNode<EnemyQueSpot> node = queue.First;
		if(node==null){
			return;
		}
		EnemyQueSpot spot = node.Value;
		queue.Remove(node);
		if(spot.position!=null){
			Vector3 position = spot.position;
			GameObject obj = MonoBehaviour.Instantiate(spot.unin,position,Quaternion.identity) as GameObject;
			BulletPool.activePool.Tele(position);
			obj.SetActive(true);
		}
		else{
			Vector3 position = ArenaProvider.active.GetRandomTile().transform.position;
			GameObject obj = MonoBehaviour.Instantiate(spot.unin,position,Quaternion.identity) as GameObject;
			BulletPool.activePool.Tele(position);
			obj.SetActive(true);
		}
	}

	public bool IsEmpty(){
		return queue.First==null;
	}

}
