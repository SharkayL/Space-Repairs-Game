using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
	
	GameObject radialPrefub;
	public static float minSize;
	public static Vector2 center;
	public static GameUI active;
	private ObjectPool spots;
	public GameObject radarDotUnin;
	private float radRatio = 0.8f;
	private float fadeDistance = 0.9f;
	private float minScale = 0.3f;
	private float scaleAwayEdge = 100f;
	private float radarRadius;
	private float radarFadeRadius;
	private Color enemyColor = new Color(1,0,0);
	private Color barrelColor = new Color(0,1,0);

	// Use this for initialization
	void Awake () {
		GameUI.active=this;
		radialPrefub = Resources.Load<GameObject>("Radial Health") as GameObject;
		spots = new ObjectPool(60);

		for(int i=0;i<60;i++){
			GameObject dot = (GameObject)Instantiate(radarDotUnin);
			spots.Add(dot.GetComponent<PooledObj>());
			dot.SetActive(false);
			dot.transform.SetParent(this.transform,false);
		}

		AudioSettings.SetDSPBufferSize(200,2);
	}

	void Start(){
		RectTransform rect = this.GetComponent<RectTransform>();
		minSize = Mathf.Min (rect.rect.width,rect.rect.height);
		center = new Vector2(rect.rect.width/2,rect.rect.height/2);
		radarRadius=(minSize/2)*radRatio;
		radarFadeRadius=(minSize/2)*fadeDistance;
	}

	public void AddPowerDial(PowerGen power){
		GameObject radial = Instantiate(radialPrefub) as GameObject;
		radial.SetActive(true);
		radial.transform.SetParent(this.transform,false);
		
		PowerHealth_UI radialUI = radial.GetComponent<PowerHealth_UI>();
		radialUI.SetCanvas(this.GetComponent<Canvas>());
		radialUI.SetWatched(power);
	}

	// Update is called once per frame
	void Update () {
		List<EnememyEntity> enemies = EnemyManager.activeManager.EnemeyList;
		spots.ReturnAll();

		foreach(EnememyEntity enemy in enemies){

			Vector2 enemyPos = Camera.main.WorldToScreenPoint(enemy.transform.position);
			float distance = (enemyPos-center).magnitude;
			if(distance<radarRadius){
				continue;
			}
			else{
				GameObject activeDot = spots.Get().gameObject;
				activeDot.SetActive(true);
				activeDot.GetComponent<Image>().color=enemyColor;
				Vector2 dotPos = (enemyPos-center).normalized*radarRadius+center;
				float actualDist = (enemy.transform.position-PlayerShip.player.transform.position).magnitude;
				actualDist = (scaleAwayEdge-actualDist)/scaleAwayEdge;
				float scale;
				if(actualDist<minScale){
					scale=minScale;
				}
				else{
					scale=actualDist;
				}

				activeDot.transform.localScale = new Vector3(scale,scale,scale);
				activeDot.transform.position=dotPos;

			}
		}

		foreach(Throwable barrel in BulletPool.activePool.barrels.GetActive()){
			Vector2 enemyPos = Camera.main.WorldToScreenPoint(barrel.transform.position);
			float distance = (enemyPos-center).magnitude;
			if(distance<radarRadius){
				continue;
			}
			else{
				GameObject activeDot = spots.Get().gameObject;
				activeDot.SetActive(true);
				activeDot.GetComponent<Image>().color=barrelColor;
				Vector2 dotPos = (enemyPos-center).normalized*radarRadius+center;
				
				activeDot.transform.position=dotPos;
				
			}
		}

	}
}
