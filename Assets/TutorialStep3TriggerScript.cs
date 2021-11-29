using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TutorialStep3TriggerScript : MonoBehaviour {
	public Text tutorialText;
	public TutorialControllerScript TutorialController;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			if(TutorialController.Finished())
			{
				Application.LoadLevel("MainmenuScene");
			}
		}
	}
	

}
