using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimerUI : MonoBehaviour {

	private Text text;
	private Image bar;

	public static TimerUI active;
	// Use this for initialization
	void Start () {
		TimerUI.active=this;
		this.text = this.transform.FindChild("timer").GetComponent<Text>();
		this.bar = this.transform.FindChild("bar").GetComponent<Image>();
	}

	public void SetTime(float time,float max){
		int seconds = Mathf.RoundToInt(time);
		int minutes = seconds/60;
		seconds-=minutes*60;
		text.text=minutes.ToString()+":"+seconds;

		float per = 1f-time/max;
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

	// Update is called once per frame
	void Update () {
	
	}
}
