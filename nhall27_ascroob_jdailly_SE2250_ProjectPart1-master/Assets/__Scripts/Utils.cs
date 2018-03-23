using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils : MonoBehaviour {

	//MATERIALS FUNCTIONS

	//returns a list of all Materials on this GameObject and its children
	static public Material[] GetAllMaterials(GameObject go){
		Renderer[] rends = go.GetComponentsInChildren<Renderer> ();

		List<Material> mats = new List<Material> ();
		foreach(Renderer rend in rends){
			mats.Add(rend.material);
		}
		return(mats.ToArray());
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
