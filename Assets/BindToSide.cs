using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BindToSide : MonoBehaviour {

	public bool left;
	public bool right;
	public float offset = 50;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(left){
			GameObject canvas = this.transform.parent.gameObject;
			
			Rect rect = canvas.GetComponent<RectTransform>().rect;
			
			Vector2 oldPos = this.transform.localPosition;
			oldPos.x=-rect.width/2+offset;
			this.transform.localPosition=oldPos;
		}

		if(right){
			GameObject canvas = this.transform.parent.gameObject;
			
			Rect rect = canvas.GetComponent<RectTransform>().rect;
			
			Vector2 oldPos = this.transform.localPosition;
			oldPos.x=rect.width/2-offset;
			this.transform.localPosition=oldPos;
		}
	}


}
