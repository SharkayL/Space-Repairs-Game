using UnityEngine;
using System.Collections;

public class Scenario3 : MonoBehaviour, IBuildOrder {
	
	public void Awake(){
		ArenaProvider.order=this;
	}
	// Use this for initialization
	void Start () {
		GameObject shootBot = Resources.Load<GameObject>("Fight_Bot") as GameObject;
		GameObject cutBot = Resources.Load<GameObject>("Cut_Bot") as GameObject;
		
		EnemyQue que = new EnemyQue();
		
		ScenarioController.active.que=que;
		que.Add (0,shootBot,new Vector3(0,110,0));
		que.Add (0,shootBot,new Vector3(0,-110,0));
		que.Add (30,shootBot,new Vector3(0,110,0));
		que.Add (0,shootBot,new Vector3(0,-110,0));
		que.Add (0,shootBot,default(Vector3));
		que.Add (30,shootBot,new Vector3(0,110,0));
		que.Add (0,shootBot,new Vector3(0,-110,0));
		que.Add (0,shootBot,default(Vector3));
		que.Add (30,shootBot,default(Vector3));
		que.Add (0,shootBot,default(Vector3));
		que.Add (0,shootBot,default(Vector3));
		que.Add (0,shootBot,default(Vector3));
		que.Add (0,shootBot,new Vector3(0,-110,0));
		que.Add (0,shootBot,new Vector3(0,110,0));
		que.Add (20,shootBot,default(Vector3));
		que.Add (0,shootBot,default(Vector3));
		que.Add (0,shootBot,default(Vector3));
		que.Add (0,shootBot,default(Vector3));
	}
	
	public void Build(){
		ArenaProvider arena = ArenaProvider.active;
		int widht = arena.columnCount;
		int height = arena.rowCount;
		
		arena.placeBulkTile(arena.powerObject,arena.columnCount/2,arena.rowCount/2-35);
		
		arena.placeBulkTile(arena.powerObject,arena.columnCount/2,arena.rowCount/2+35);
		
		arena.placeTile(arena.dispenserObject,widht/2,height/2+3);
		arena.placeTile(arena.dispenserObject,widht/2,height/2+31);
		arena.placeTile(arena.dispenserObject,widht/2,height/2-3);
		arena.placeTile(arena.dispenserObject,widht/2,height/2-31);
	}
	
	
	// Update is called once per frame
	void Update () {
		
	}
}
