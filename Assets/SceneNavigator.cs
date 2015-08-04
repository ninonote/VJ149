using UnityEngine;
using System.Collections;

public class SceneNavigator : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown ("c")) {
			Application.LoadLevel("LiveCamera");
		} else if (Input.GetKeyDown ("w")) {
			Application.LoadLevel ("AudioWave3D");
		}
	}
}
