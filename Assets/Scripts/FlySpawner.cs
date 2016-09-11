using UnityEngine;
using System.Collections;

public class FlySpawner : MonoBehaviour {

	public GameObject flyObj;
	public GameObject[] flyLocationObjects;

	private static Vector3[] flyLocations;
	private static GameObject fly;

	// Use this for initialization
	void Awake(){
		flyLocations = new Vector3[flyLocationObjects.Length];
		for(int k = 0; k < flyLocationObjects.Length; k++){
			flyLocations[k] = flyLocationObjects[k].transform.position;
		}

		fly = flyObj;
	}

	void Start(){
		SpawnNewfly();
	}

	public static void SpawnNewfly(){
		Vector3 flyPosition = flyLocations[Random.Range(0, flyLocations.Length)];
		GameObject go = Instantiate(fly, flyPosition, Quaternion.identity) as GameObject;
		GameObject UpAndDownGuy = GameObject.Find("UpAndDownGuy");
		go.transform.parent = UpAndDownGuy.transform;
	}
}
