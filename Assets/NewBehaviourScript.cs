using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

	private GameObject enemy;
	private Vector2 center = new Vector2(0,0);
	private float radius = 10f;
	private float counter = 0f;
	private float LIMIT = Mathf.PI*2;
	private float angAcc = 0f;
	private float angAccInc = 0.1f;
	private Vector3 transformVector = new Vector3(0,0,0);
	private GameObject trail;
	// Use this for initialization
	void Start () {
		Debug.Log ("Test");
		//trail = Instantiate(Resources.Load("Prefab_Particle")) as GameObject;
		trail = Instantiate (Resources.Load ("Prefab_Particle")) as GameObject;
		trail.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		bool forward = Input.GetKey (KeyCode.W);
		if (forward) {
			counter += LIMIT/120;
			counter = counter % LIMIT;
			transformVector.Set (center.x + Mathf.Sin (counter) * radius, 0, 0);
			this.transform.transform.position = transformVector;
		}

		forward = Input.GetKey (KeyCode.UpArrow);
		bool left = Input.GetKey (KeyCode.LeftArrow);
		bool right = Input.GetKey (KeyCode.RightArrow);

		if ((left || right) && !(left && right)) {

			float angleChange = 0f;
			if (left) {
				angleChange += 1f + angAcc;
			} else {
				angleChange -= 1f + angAcc;
			}

			angAcc += angAccInc;
			transformVector.Set (0, 0, angleChange);

			this.transform.Rotate (transformVector);

		} else {
			angAcc=0f;
		}

		if (forward) {
			float currentAngle = (this.transform.eulerAngles.z)*Mathf.Deg2Rad;

			transformVector.Set(0.1f*Mathf.Cos(currentAngle),0.1f*Mathf.Sin(currentAngle),0);
			this.transform.Translate(transformVector,null);

			GameObject newObject = (GameObject)Instantiate(trail,this.transform.position,this.transform.rotation);
			newObject.SetActive(true);
		}



	}
}
