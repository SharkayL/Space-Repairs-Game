using UnityEngine;
using System.Collections;

public class Scenario1 : MonoBehaviour, IBuildOrder {

	public void Awake(){
		ArenaProvider.order=this;
	}
	// Use this for initialization
	void Start () {
		GameObject shootBot = Resources.Load<GameObject>("Fight_Bot") as GameObject;
		GameObject cutBot = Resources.Load<GameObject>("Cut_Bot") as GameObject;

		EnemyQue que = new EnemyQue();

		ScenarioController.active.que=que;
		que.Add(2,shootBot,new Vector3(35,0,0));
		que.Add(0,shootBot,new Vector3(-35,0,0));
		que.Add(50,cutBot,default(Vector3));
	}

	public void Build(){
		ArenaProvider arena = ArenaProvider.active;
		int widht = arena.columnCount;
		int height = arena.rowCount;

		arena.placeBulkTile(arena.powerObject,arena.columnCount/2+15,arena.rowCount/2);

		arena.placeBulkTile(arena.powerObject,arena.columnCount/2-15,arena.rowCount/2);

		arena.placeTile(arena.dispenserObject,widht/2,height/2+3);
	}


	// Update is called once per frame
	void Update () {
	
	}
}
