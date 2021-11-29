using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class FixUI : MonoBehaviour {

	public static FixUI active;
	public float looseRatio = 0.75f;
	private int maxHealth;
	private List<PowerGen> generators = new List<PowerGen>(); 
	private Image bar;

	// Use this for initialization
	void Awake(){
		FixUI.active=this;
	}

	void Start () {
		bar = this.transform.FindChild("bar").GetComponent<Image>();
	}

	public void Register(PowerGen gen){
		generators.Add(gen);
		maxHealth+=gen.maxHitpoints;
	}
	
	// Update is called once per frame
	void Update () {

		int currentHealth = 0;

		foreach(PowerGen gen in generators){
			currentHealth+=gen.HealthPoints();
		}

		float per = (float)(currentHealth-maxHealth*looseRatio)/(maxHealth*looseRatio);
		if(per<0){
			ScenarioController.active.Fail();
		}

		bar.fillAmount=per;
		
		if(per<0.5){
			float relValue = per/0.5f;
			bar.color = new Color(1f,relValue,0);
		}
		else{
			float relValue = (per-0.5f)/0.5f;
			bar.color = new Color((1f-relValue),1f,0);
		}

	}
}
