using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WinBox : MonoBehaviour {

	public bool finalLevel = false;
	public string nextLevel;
	public static WinBox active;

	// Use this for initialization
	void Start () {
		WinBox.active=this;
		this.gameObject.SetActive(false);
		if(!finalLevel){
			Button next = this.transform.FindChild("next").GetComponent<Button>();
			
			next.onClick.AddListener(() => {
				Application.LoadLevel(nextLevel);
			});
		}




		Button menu = this.transform.FindChild("menu").GetComponent<Button>();
		
		menu.onClick.AddListener(() => {
			Application.LoadLevel("MainmenuScene");
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
