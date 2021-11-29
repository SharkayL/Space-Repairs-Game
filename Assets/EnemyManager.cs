using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyManager : MonoBehaviour {

	private List<EnememyEntity> enemyList = new List<EnememyEntity>();
	private List<EnememyEntity> deadList = new List<EnememyEntity>();

	public List<GameObject> targets = new List<GameObject>();

	private GameObject enemy1;
	public static EnemyManager activeManager;

	void Awake() {
		EnemyManager.activeManager=this;
	}

	public void AddEnemy(EnememyEntity enemy){
		enemyList.Add(enemy);
	}

	public void RemoveEnemy(EnememyEntity enemy){
		deadList.Add(enemy);
	}

	public List<EnememyEntity> EnemeyList {
		get {
			return enemyList;
		}
	}

	// Use this for initialization
	void Start () {
		targets.Add (GameObject.FindWithTag("Player"));
		enemy1 = Resources.Load<GameObject>("Fight_Bot") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
		while(deadList.Count!=0){
			enemyList.Remove(deadList[0]);
			deadList.RemoveAt(0);
		}

	}
}
