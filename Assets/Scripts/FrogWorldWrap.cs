using UnityEngine;
using System.Collections;

public class FrogWorldWrap : MonoBehaviour {

	public GameObject wwObj;

	private GameObject[] ww;
	private float screenHeight, screenWidth;
	private FrogController FC;

	// Use this for initialization
	void Start () {
		FC = gameObject.GetComponent<FrogController>();

		screenHeight = 2f * Camera.main.orthographicSize;
		screenWidth = screenHeight * Camera.main.aspect;
		
		SetupWorldWrap();
	}
	
	// Update is called once per frame
	void Update () {
		TryToSwapShips();
	}

	void SetupWorldWrap(){
		ww = new GameObject[2];

		//right ghost
		ww[0] = Instantiate(wwObj, Vector3.zero, Quaternion.identity) as GameObject;
		FrogGhost ghost0 = ww[0].GetComponent<FrogGhost>();
		ghost0.SetRealFrog(gameObject);
		ghost0.SetOffset(screenWidth);

		//left ghost
		ww[1] = Instantiate(wwObj, Vector3.zero, Quaternion.identity) as GameObject;
		FrogGhost ghost1 = ww[1].GetComponent<FrogGhost>();
		ghost1.SetRealFrog(gameObject);
		ghost1.SetOffset(-screenWidth);

		FrogGhost[] ghosties = new FrogGhost[2]{ghost0, ghost1};

		FC.SetGhosts(ghosties);

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
				FC.TeleportFlies();
				PositionShips();
			}
		}
	}
}
