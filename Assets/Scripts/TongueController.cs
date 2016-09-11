using UnityEngine;
using System.Collections;

public class TongueController : MonoBehaviour {

	public GameObject frog;
	public float cooldown = 0.2f;
	private float startTime, deltaTime;
	private FrogController FC;
	private bool JustAte;

	private int playerId;

	void Start(){
		if(frog != null){
			FC = frog.GetComponent<FrogController>();
			playerId = FC.GetPlayerId();
		}
	}

	void OnTriggerEnter2D(Collider2D coll){
		if(coll.gameObject.tag=="Fly"){
			FlyController FlyC = coll.gameObject.GetComponent<FlyController>();
			if(FlyC.GetPlayerId() != playerId){
				if(!JustAte){
					FC.GainFly(coll.gameObject);
					JustAte = true;
					startTime = Time.time;
				}
			}
		}
	}

	void Update(){
		deltaTime = Time.time - startTime;
		if (JustAte && deltaTime >= cooldown){
			JustAte = false;
		}
	}

	public GameObject GetFrog(){
		return frog;
	}

	public void SetFrog(GameObject frog){
		this.frog = frog;
		FC = frog.GetComponent<FrogController>();
		playerId = FC.GetPlayerId();
	}
}
