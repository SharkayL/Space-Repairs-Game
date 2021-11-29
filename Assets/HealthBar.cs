using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

	public static HealthBar active;
	private Image bar;

	void Awake(){
		HealthBar.active=this;
	}

	// Use this for initialization
	void Start () {
		bar = this.transform.FindChild("bar").GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		float value = PlayerShip.player.GetComponent<PlayerShip>().GetHealth();
		//Debug.Log ("H:"+value);
		if(value<0.5){
			float relValue = value/0.5f;
			bar.color = new Color(1f,relValue,0);
		}
		else{
			float relValue = (value-0.5f)/0.5f;
			bar.color = new Color((1f-relValue),1f,0);
		}
		
		bar.fillAmount = value;
	}
}
