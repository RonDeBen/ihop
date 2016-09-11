using UnityEngine;
using System.Collections;
using Rewired;

public class FrogGhost : MonoBehaviour {

	public GameObject tongue, tongueAnchor;

	private GameObject realFrog;
	private float offset;
	private Rigidbody2D realRB, ghostRB;
	private SpriteRenderer realSprender, ghostSprender, tongueSprender;
	private FrogController FC;
	private BoxCollider2D tongueBoxCollider;

	private bool facingRight = true;
	private bool isTonguing = false;
	private bool tonguePressed = false;
	private Player player;

	private float tongueStartTime, tongueDeltaTime, maxTongueScale, timeToTongue, tongueTheta;

	void Start(){
		tongueSprender = tongue.GetComponent<SpriteRenderer>();
		tongueBoxCollider = tongue.GetComponent<BoxCollider2D>();
	}
	
	// Update is called once per frame
	void Update() {
		if(realFrog != null){
			tonguePressed = player.GetButtonDown("Tongue");
			if(facingRight && realRB.velocity.x < 0){
				facingRight = false;
				tongueAnchor.transform.localPosition = Vector3.Scale(tongueAnchor.transform.localPosition, Vector3.left);

			}else if(!facingRight && realRB.velocity.x > 0){
				facingRight = true;
				tongueAnchor.transform.localPosition = Vector3.Scale(tongueAnchor.transform.localPosition, Vector3.left);
			}

			gameObject.transform.position = realFrog.transform.position + (Vector3.right * offset);
			ghostSprender.flipX = realSprender.flipX;

			if(tonguePressed && !isTonguing){
				isTonguing = true;
				tongueStartTime = Time.time;
				tongueTheta = FC.GetTongueTheta();
				tongueSprender.enabled = true;
				tongueBoxCollider.enabled = true;
				tongue.transform.eulerAngles = new Vector3(0.0f, 0.0f, tongueTheta);
			}
			if (isTonguing){
				tongueExtend();
			}
		}
	}

	public void SetRealFrog(GameObject frog){
		realFrog = frog;
		// gameObject.transform.localScale = realFrog.transform.localScale;

		realRB = realFrog.GetComponent<Rigidbody2D>();
		ghostRB = gameObject.GetComponent<Rigidbody2D>();

		realSprender = realFrog.GetComponent<SpriteRenderer>();
		ghostSprender = gameObject.GetComponent<SpriteRenderer>();

		FC = frog.GetComponent<FrogController>();
		maxTongueScale = FC.GetMaxTongueScale();
		timeToTongue = FC.GetTimeToTongue();
		tongueAnchor.transform.localPosition = FC.GetTongueAnchorPosition();
		tongue.GetComponent<TongueController>().SetFrog(realFrog);
		player = ReInput.players.GetPlayer(FC.GetPlayerId());
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

		tongue.transform.position = tongueAnchor.transform.position + new Vector3(tongueX, tongueY, 0.0f);

		if(tongueDeltaTime/timeToTongue > 1){
			isTonguing = false;
			tongueBoxCollider.enabled = false;
			tongueSprender.enabled = false;
			tongue.transform.localScale = Vector3.one;
			tongue.transform.position = tongueAnchor.transform.position;
		}
	}

	public void SetOffset(float offset){
		this.offset = offset;
	}
}
