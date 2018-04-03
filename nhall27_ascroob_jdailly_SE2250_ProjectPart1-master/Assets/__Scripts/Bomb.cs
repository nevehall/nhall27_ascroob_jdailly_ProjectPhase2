using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour {
	[Header("Set In Inspector")]
	public float growfactor = 1f;

	private float maxSize = 220f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float timer = 0;

		timer += Time.deltaTime;
		transform.localScale += new Vector3(1,1,0) * Time.deltaTime * growfactor;

		if (gameObject.transform.localScale.y >= maxSize) {
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter(Collider go){
		GameObject otherGo = go.gameObject;

		if (otherGo.tag == "Enemy") {
			Destroy(otherGo.gameObject);
		}
	}
}


