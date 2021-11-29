using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FailBox : MonoBehaviour {

	public static FailBox active;
	private Button retry;


	void Awake(){
		FailBox.active=this;
	}

	// Use this for initialization
	void Start () {
		this.gameObject.SetActive(false);
		retry = this.transform.FindChild("retry").GetComponent<Button>();

		retry.onClick.AddListener(() => {
			Application.LoadLevel(Application.loadedLevel);
		});

		Button menu = this.transform.FindChild("menu").GetComponent<Button>();
		
		menu.onClick.AddListener(() => {
			Application.LoadLevel("MainmenuScene");
		});
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
