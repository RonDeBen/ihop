using UnityEngine;
using System.Collections;

public class FlyGhost : MonoBehaviour {

	private GameObject shadowFly;
	private SpriteRenderer shadowSprender, flySprender;
	private float offset;
	
	// Update is called once per frame
	void FixedUpdate () {
		if(shadowFly != null){
			flySprender.flipX = shadowSprender.flipX;
			flySprender.sprite = shadowSprender.sprite;
			gameObject.transform.position = shadowFly.transform.position + (Vector3.right * offset);
		}else{
			Destroy(gameObject);
		}
	}

	public void SetShadowFly(GameObject shadowFly){
		this.shadowFly = shadowFly;
		shadowSprender = shadowFly.GetComponent<SpriteRenderer>();
		flySprender = gameObject.GetComponent<SpriteRenderer>();
	}

	public void SetOffset(float offset){
		this.offset = offset;
	}

	public GameObject GetShadowFly(){
		return shadowFly;
	}

	public int GetPlayerId(){
		return shadowFly.GetComponent<FlyController>().GetPlayerId();
	}


}
