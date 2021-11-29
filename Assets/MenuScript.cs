using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour {
	private AudioSource sound, sound1;
	private AudioSource[] audios;

	public void Start(){
		audios = GetComponents<AudioSource>();
		sound = audios [0];
		sound1 = audios [1];
	}

	public void StartGame()	{
		sound1.Play();
		Application.LoadLevel("scene1");
	}

	public void EndGame() {		
		sound1.Play();
		Application.Quit ();
	}
	
	public void playSound(){
		sound.Play();
	}

	public void startTut(){
		sound1.Play();
		Application.LoadLevel("tutorialScene");
	}
}
