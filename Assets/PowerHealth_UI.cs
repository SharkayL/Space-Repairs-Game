using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PowerHealth_UI : MonoBehaviour {

	private Image radial;
	private float value = 0f;
	private PowerGen gen;
	private Canvas canvas;
	private Text text;
	private float edgeOffset = 150f;
	private float leftRightOffset = 200f;
	private float relDistance = 0.9f;
	private float minSizeCap = 0.5f;
	// Use this for initialization
	void Start () {

		radial = this.transform.FindChild("Radial Dial").GetComponent<Image>();
		text = this.transform.FindChild("Text").GetComponent<Text>();
	}

	public void SetWatched(PowerGen gen){
		this.gen=gen;
		gen.ui=this.gameObject;
	}
	public void SetCanvas(Canvas canvas){
		this.canvas=canvas;
	}
	// Update is called once per frame
	void Update () {
		this.SetPercent(gen.Health());

		Vector2 powerPos = Camera.main.WorldToScreenPoint(gen.transform.position);
		float distance = (GameUI.center - powerPos).magnitude/GameUI.minSize;

		float actualDist = (PlayerShip.player.transform.position-gen.transform.position).magnitude;
		text.text=""+Mathf.RoundToInt(actualDist*10);
		if(distance>1f){
			distance=1f;
		}
		else if(distance<minSizeCap){
			distance=minSizeCap;
		}


		Vector3 scale = new Vector3(distance,distance,distance);

		this.transform.localScale=(scale);

		float targetPosX = powerPos.x;
		float targetPosY = powerPos.y;
		RectTransform rect = canvas.GetComponent<RectTransform>();
		float width = rect.rect.width;
		float height = rect.rect.height;

		if(targetPosX<leftRightOffset){
			targetPosX=leftRightOffset;
		}
		else if(targetPosX>(width-leftRightOffset)){
			targetPosX=width-leftRightOffset;
		}

		if(targetPosY<edgeOffset){
			targetPosY=edgeOffset;
		}
		else if(targetPosY>(height-edgeOffset)){
			targetPosY=height-edgeOffset;
		}
		powerPos.x=targetPosX;
		powerPos.y=targetPosY;
		this.transform.position=powerPos;


	}

	public void SetPercent(float value){
		if(value<0.5){
			float relValue = value/0.5f;
			radial.color = new Color(1f,relValue,0);
		}
		else{
			float relValue = (value-0.5f)/0.5f;
			radial.color = new Color((1f-relValue),1f,0);
		}

		radial.fillAmount = value;
	}

}
