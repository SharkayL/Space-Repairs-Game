using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialControllerScript : MonoBehaviour {
	public Text tutorialText;
	public Fight_Bot fightBot;
	bool lastStepFinished;
	// Use this for initialization
	void Start () {
		lastStepFinished = false;
	}
	
	// Update is called once per frame
	void Update () {

		bool enabled = fightBot.isActiveAndEnabled;
		if (!enabled) {
			Finished ();
			tutorialText.text = "Great! Now just repair all your generators. Do this simply by flying close to them and letting your ship do the work. Once you've done this, fly around for a while and play with the controls, then fly to the left of the area to finish the tutorial.";
		}
	}

	public void lastStepComplete()
	{
		lastStepFinished = true;
	}

	public bool Finished()
	{
		return lastStepFinished;
	}
}
