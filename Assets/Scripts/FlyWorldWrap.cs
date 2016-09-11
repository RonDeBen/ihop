using UnityEngine;
using System.Collections;

public class FlyWorldWrap : MonoBehaviour {

	public GameObject wwObj;

	private GameObject[] ww;
	private float screenHeight, screenWidth;

	// Use this for initialization
	void Start () {
		screenHeight = 2f * Camera.main.orthographicSize;
		screenWidth = screenHeight * Camera.main.aspect;
		
		SetupWorldWrap();
	}
	
	// Update is called once per frame
	void Update () {
		// TryToSwapShips();
	}

	void SetupWorldWrap(){
		ww = new GameObject[2];

		//right ghost
		ww[0] = Instantiate(wwObj, Vector3.zero, Quaternion.identity) as GameObject;
		ww[0].GetComponent<FlyGhost>().SetShadowFly(gameObject);
		ww[0].GetComponent<FlyGhost>().SetOffset(screenWidth);

		//left ghost
		ww[1] = Instantiate(wwObj, Vector3.zero, Quaternion.identity) as GameObject;
		ww[1].GetComponent<FlyGhost>().SetShadowFly(gameObject);
		ww[1].GetComponent<FlyGhost>().SetOffset(-screenWidth);

		PositionShips();
	}

	void PositionShips(){
		ww[0].transform.position = gameObject.transform.position + (Vector3.right * screenWidth);
		ww[1].transform.position = gameObject.transform.position + (Vector3.left * screenWidth);
	}

	void TryToSwapShips(){
		foreach(GameObject ghost in ww){
			if(ghost.transform.position.x < screenWidth && ghost.transform.position.x > 0.0f){
				transform.position = ghost.transform.position;
				PositionShips();
			}
		}
	}
}
