using UnityEngine;
using System.Collections;

public class CenterScreen : MonoBehaviour {

	// Use this for initialization
	void Awake () {
		float screenHeight = 2f * GetComponent<Camera>().orthographicSize;
		float screenWidth = screenHeight * GetComponent<Camera>().aspect;

		transform.position = new Vector3(screenWidth / 2f, screenHeight / 2f, transform.position.z);

	}

	void Start(){
		MusicMiddleware.loopSound("Hop Skip Swing", true);
	}
}