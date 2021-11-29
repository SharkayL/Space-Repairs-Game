using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class TutorialStep2TriggerScript : MonoBehaviour {
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
				tutorialText.text = "Looks like your generator is getting hit! Pick up an object to fire at the enemy by flying face-first into it, and fire with the space bar. " +
					"Your health is represented at the bottom of the screen, and your Capital Ship's health represented to the right.";
				Destroy (this.gameObject);
			TutorialController.lastStepComplete();
		}
	}
}
