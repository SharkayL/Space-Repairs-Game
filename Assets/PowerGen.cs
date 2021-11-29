using UnityEngine;
using System.Collections;

public class PowerGen : MonoBehaviour,IDamage,IFixable {

	public int maxHitpoints = 500;
	private int health;
	public bool lost = false;
	public GameObject ui;

	// Use this for initialization
	void Start () {
		this.health=maxHitpoints;
		GameUI ui = GameObject.FindWithTag("UI").GetComponent<GameUI>();
		EnemyManager.activeManager.targets.Add (this.gameObject);
		ui.AddPowerDial(this);
		this.GetComponent<DamagebleObj>().RegisterReciver(this);
		this.GetComponent<FixableObj>().RegisterReciver(this);

		FixUI.active.Register(this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void DamageBy (int hits)
	{
		this.health-=hits;
		if(this.health<0){
			this.lost=true;
			ui.SetActive(false);
			EnemyManager.activeManager.targets.Remove(this.gameObject);
		}
	}

	public void FixBy(int by){
		if(lost){
			return;
		}
		health+=by;
		if(health>maxHitpoints){
			health=maxHitpoints;
		}
	}

	public float Health(){
		return (float)health/maxHitpoints;
	}

	public int HealthPoints(){
		return health;
	}

	public GameObject GetGameObject(){
		return this.gameObject;
	}
}
