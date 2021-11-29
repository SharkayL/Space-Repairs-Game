using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TutorialStep1TriggerScript : MonoBehaviour {
	
	public Text tutorialText;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Player") {
			tutorialText.text = "Good job! Red dots on your screen indicate enemy positions. Green dots indicate objects you may use as weapons. The lightning bolts indicate the positions of your generators, which you must keep as repaired as possible. Investigate the generator in the bottom right corner of the map!";
			Destroy (this.gameObject);
		}
	}
}
