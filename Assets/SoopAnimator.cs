using UnityEngine;
using System.Collections;

public class SoopAnimator : MonoBehaviour {

	private Material primaryMaterial;
	private float offset = 0f;
	// Use this for initialization
	void Start () {
		primaryMaterial = this.gameObject.renderer.materials[1];
	}
	
	// Update is called once per frame
	void Update () {
		offset=(offset+Time.deltaTime)%512f;
		primaryMaterial.SetTextureOffset("_MainTex",new Vector2(offset,0));
	}
}
