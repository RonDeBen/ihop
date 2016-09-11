using UnityEngine;
using System.Collections;

public class FlyController : MonoBehaviour {

	public GameObject follow;
	public int positionInLine = 0;
	public float displacementBetweenFlies = 1; 
	public float timeToReachFrog = 0.1f;
	private float startTime, deltaTime;
	private bool isLeaping = false;
	private Vector3 startPos, endPos;
	private SpriteRenderer hostSprender, flySprender;
	public int playerId = -1;
	private GameObject host;

	// Use this for initialization
	void Start () {
		flySprender = gameObject.GetComponent<SpriteRenderer>();
	}
	
	void FixedUpdate () {
		if(follow != null){
			if (!isLeaping && (Vector3.Distance(transform.position, restingState()) > 0.2f)){
				isLeaping = true;
				startTime = Time.time;
				startPos = transform.position;
				endPos = restingState();
			}

			flySprender.flipX = hostSprender.flipX;

			leap();
		}
	}

	Vector3 restingState(){
		if (hostSprender.flipX){//the frog is looking left
			return (follow.transform.position + (displacementBetweenFlies * Vector3.left));
		}else{
			return (follow.transform.position + (displacementBetweenFlies * Vector3.right));
		}
	}

	void leap(){
		deltaTime = Time.time - startTime;
		transform.position = Vector3.Lerp(startPos, endPos, (deltaTime/timeToReachFrog));
		if(deltaTime/timeToReachFrog >= 1){
			isLeaping = false;
		}
	}

	public void SwapHost(GameObject newHost){
		if(host != null){
			host.GetComponent<FrogController>().LoseFly(positionInLine);
		}else{
			FlySpawner.SpawnNewfly();
		}
		FrogController FC = newHost.GetComponent<FrogController>();
		follow = FC.GetNextInLine();
		host = newHost;
		hostSprender = host.GetComponent<SpriteRenderer>();
		playerId = FC.GetPlayerId();
		positionInLine = FC.GetPosition();
	}

	public void SwapFollowing(GameObject newTarget){
		follow = newTarget;
	}

	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Tongue"){
			coll.gameObject.GetComponent<TongueController>();
		}
	}

	public void SetPosition(int position){
		positionInLine = position;
	}

	public int GetPlayerId(){
		return playerId;
	}

	public void SetPlayerId(int newId){
		playerId = newId;
	}
}
