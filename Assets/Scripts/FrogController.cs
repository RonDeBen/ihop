using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Rewired;

public class FrogController : MonoBehaviour {

	public GameObject groundCheckLeftObj, groundCheckRightObj, arrow, tongue, tongueAnchor, arrowAnchor;
	public float startForce, maxForce, timeToMax, timeToForceJump;
	public float maxTongueScale, timeToTongue;
	public int numberOfFliesForDeathTouch = 5;
	private float chargeStartTime, jumpDeltaTime, x, y;
	private float tongueStartTime, tongueDeltaTime;
	private SpriteRenderer arrowSprender, frogSprender;
	private bool groundedLeft = false;
	private bool groundedRight = false;
	private bool grounded = false;
	private bool isCharging = false;
	private bool isTounging = false;

	private bool isDeathTouch = false;

	private int numberOfFlies;
	private List<GameObject> myFlies = new List<GameObject>();

	private Vector2 lastDirection;

	public int playerId = 0;
	private Player player;

	private bool jumpPressed, jumpReleased, tonguePressed;

	private bool facingRight = true;

	private Transform groundCheckLeft, groundCheckRight;
	private Rigidbody2D rb;

	private Vector3 arrowScale;

	private Vector3 moveVector;

	private SpriteRenderer tongueSprender;
	private float tongueTheta;
	private BoxCollider2D tongueBoxCollider;

	// Use this for initialization
	void Start () {
		groundCheckLeft = groundCheckLeftObj.transform;
		groundCheckRight = groundCheckRightObj.transform;
		rb = gameObject.GetComponent<Rigidbody2D>();
		arrowSprender = arrow.GetComponent<SpriteRenderer>();
		frogSprender = gameObject.GetComponent<SpriteRenderer>();
		player = ReInput.players.GetPlayer(playerId);

		tongueSprender = tongue.GetComponent<SpriteRenderer>();

		arrowScale = arrow.transform.localScale;

		tongueBoxCollider = tongue.GetComponent<BoxCollider2D>();
	}
	
	void Update() {

		if(facingRight && rb.velocity.x < 0){
			facingRight = false;
			frogSprender.flipX = false;
			tongueAnchor.transform.localPosition = Vector3.Scale(tongueAnchor.transform.localPosition, Vector3.left);

		}else if(!facingRight && rb.velocity.x > 0){
			facingRight = true;
			frogSprender.flipX = true;
			tongueAnchor.transform.localPosition = Vector3.Scale(tongueAnchor.transform.localPosition, Vector3.left);
		}

		x = player.GetAxis("Move Horizontal");
		y = player.GetAxis("Move Vertical");

		if (!Mathf.Approximately(x, 0.0f)){
			moveVector.x = x;
		}

		if (!Mathf.Approximately(y, 0.0f)){
			moveVector.y = y;
		}

		jumpPressed = player.GetButtonDown("Jump");
		jumpReleased = player.GetButtonUp("Jump");
		tonguePressed = player.GetButtonDown("Tongue");

		groundedLeft = Physics2D.Linecast(gameObject.transform.position, groundCheckLeft.position, (1 << LayerMask.NameToLayer("floor")) | (1<<LayerMask.NameToLayer("wall")));
		groundedRight = Physics2D.Linecast(gameObject.transform.position, groundCheckRight.position, (1 << LayerMask.NameToLayer("floor")) | (1<<LayerMask.NameToLayer("wall")));
		grounded = (groundedLeft || groundedRight);

		if(jumpPressed && grounded && !isCharging){
			chargeStartTime = Time.time;
			isCharging = true;
			arrowSprender.enabled = true;
		}

		if(jumpReleased && isCharging && grounded){
			jump();
		}

		if (isCharging){
			charging();
		}

		if(tonguePressed && !isTounging && !isDeathTouch && !isCharging){
			isTounging = true;
			tongueStartTime = Time.time;
			tongue.GetComponent<SpriteRenderer>().enabled = true;
			tongueBoxCollider.enabled = true;
			tongueTheta = (Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg);
			tongue.transform.eulerAngles = new Vector3(0.0f, 0.0f, tongueTheta);
		}

		if (isTounging){
			tongueExtend();
		}
	}

	void charging(){
		jumpDeltaTime = Time.time - chargeStartTime;
		float scaleIncrease = Mathf.Lerp(1.0f, 2.0f, Mathf.Clamp(jumpDeltaTime/timeToMax, 0, 1));
		arrow.transform.localScale = arrowScale * scaleIncrease;

		float arrowX = 0.0f;
		float arrowY = 0.0f;
		float halfArrowLength = 0.0f;

		if(arrowSprender.bounds.size.x > arrowSprender.bounds.size.y){
			halfArrowLength = (arrowSprender.bounds.size.x / 2.0f);
		}else{
			halfArrowLength = (arrowSprender.bounds.size.y / 2.0f);
		}

		arrowX = moveVector.x*halfArrowLength;
		arrowY = moveVector.y*halfArrowLength;

		arrow.transform.position = (arrowAnchor.transform.position + new Vector3(arrowX, arrowY, 0.0f));

		tongueTheta = (Mathf.Atan2(moveVector.y, moveVector.x) * Mathf.Rad2Deg);
		arrow.transform.eulerAngles = new Vector3(0.0f, 0.0f, tongueTheta);
	}

	void jump(){
		float force = Mathf.Lerp(startForce, maxForce, Mathf.Clamp(jumpDeltaTime/timeToMax, 0, 1));
		rb.AddForce(moveVector * force);
		arrowSprender.enabled = false;
		arrow.transform.localScale = arrowScale;
		isCharging = false;
	}

	void tongueExtend(){
		tongueDeltaTime = Time.time - tongueStartTime;

		tongue.transform.localScale = new Vector3(Mathf.Lerp(1.0f, maxTongueScale, tongueDeltaTime/timeToTongue), 1.0f, 0.0f);
		float tongueX = 0.0f;
		float tongueY = 0.0f;
		float halfTongueWidth = 0.0f;
		if(tongueSprender.bounds.size.x > tongueSprender.bounds.size.y){
			halfTongueWidth = (tongueSprender.bounds.size.x / 2.0f);
		}else{
			halfTongueWidth = (tongueSprender.bounds.size.y / 2.0f);
		}

		tongueX = Mathf.Cos(tongueTheta*Mathf.Deg2Rad)*halfTongueWidth;
		tongueY = Mathf.Sin(tongueTheta*Mathf.Deg2Rad)*halfTongueWidth;

		tongue.transform.position = (tongueAnchor.transform.position + new Vector3(tongueX, tongueY, 0.0f));

		if(tongueDeltaTime/timeToTongue > 1){
			isTounging = false;
			tongueBoxCollider.enabled = false;
			tongueSprender.enabled = false;
			tongue.transform.localScale = Vector3.one;
			tongue.transform.position = tongueAnchor.transform.position;
		}
	}

	public void EnterDeathTouch(){
		gameObject.layer = LayerMask.NameToLayer("death touch");
		isDeathTouch = true;
		frogSprender.color = new Color(153, 50, 204, 255);
		//change to whatever animation thing
	}

	public void ExitDeathTouch(){
		gameObject.layer = LayerMask.NameToLayer("frog");
		isDeathTouch = false;
		frogSprender.color = Color.white;
		//go back to whatever animation thing
	}

	void OnCollisionEnter2D(Collision2D other){
		if(isDeathTouch && other.gameObject.tag.Equals("Frog")){
			other.gameObject.GetComponent<FrogController>().DestroyAllFlies();
			Destroy(other.gameObject);
			DestroyAllFlies();
			ExitDeathTouch();
		}
	}

	public void DestroyAllFlies(){
		for(int k = myFlies.Count - 1; k >= 0; k--){
			Destroy(myFlies[k]);
			myFlies.RemoveAt(myFlies.Count - 1);
		}
	}

	public void GainFly(GameObject fly){
		fly.GetComponent<FlyController>().SwapHost(gameObject);
		myFlies.Add(fly);
		if(myFlies.Count == numberOfFliesForDeathTouch){
			EnterDeathTouch();
		}
	}

	public void LoseFly(int position){
		myFlies.RemoveAt(position);
		ReorderFlies();
		if(isDeathTouch && myFlies.Count < numberOfFliesForDeathTouch){
			ExitDeathTouch();
		}
	}

	void ReorderFlies(){
		for(int k = myFlies.Count - 1; k >= 0; k--){
			FlyController FC = myFlies[k].GetComponent<FlyController>();
			if(k == 0){
				FC.SwapFollowing(gameObject);
			}else{
				FC.SwapFollowing(myFlies[k-1]);
			}
			FC.SetPosition(k);
		}
	}

	public GameObject GetNextInLine(){
		if(myFlies.Count == 0){
			return gameObject;
		}else{
			return myFlies[myFlies.Count-1];
		}
	}

	public int GetPosition(){
		return myFlies.Count;
	}

	public int GetPlayerId(){
		return playerId;
	}


}
