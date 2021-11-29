using UnityEngine;
using System.Collections;

public interface IBuildOrder{

	void Build();

}

public class ScenarioController : MonoBehaviour {

	public float surivalTime = 10;
	public bool quickDeploy = true;
	private float maxTime;
	public EnemyQue que;
	public bool autoResolve = true;
	public static ScenarioController active;
	// Use this for initialization

	void Awake(){
		ScenarioController.active=this;
	}

	void Start () {
		maxTime=surivalTime;
		Time.timeScale=1f;
	}


	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Escape)){
			Application.LoadLevel("mainmenuScene");
		}
		
		surivalTime-=Time.deltaTime;
		TimerUI.active.SetTime(surivalTime,maxTime);
		que.Tick();

		if(que.IsEmpty()&&EnemyManager.activeManager.EnemeyList.Count==0){
			//Debug.Log("Win");
			Win ();
		}
		else{
			if(EnemyManager.activeManager.EnemeyList.Count==0&&quickDeploy){

				que.Force();
			}
		}
	}

	public void Fail(){
		if(!autoResolve){
			return;
		}

		GameUI.active.gameObject.SetActive(false);

		GameObject player = PlayerShip.player.gameObject;
		player.SetActive(false);
		BulletPool.activePool.Explode(player.transform.position,6f);
		Time.timeScale=0.1f;
		
		foreach(EnememyEntity enemy in EnemyManager.activeManager.EnemeyList){
			enemy.gameObject.SetActive(false);
			BulletPool.activePool.Explode(enemy.transform.position,2f);
		}

		FailBox.active.gameObject.SetActive(true);
	}

	public void Win(){
		if(!autoResolve){
			return;
		}


		foreach(EnememyEntity enemy in EnemyManager.activeManager.EnemeyList){
			enemy.gameObject.SetActive(false);
			BulletPool.activePool.Explode(enemy.transform.position,2f);
		}

		WinBox.active.gameObject.SetActive(true);
	}
}
