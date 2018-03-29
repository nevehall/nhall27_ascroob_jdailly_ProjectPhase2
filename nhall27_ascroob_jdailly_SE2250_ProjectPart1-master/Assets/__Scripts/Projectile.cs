using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {
	private BoundsCheck bndCheck;
	private Renderer rend;

	[Header("These fields are set dynamically")]

	public Rigidbody rigid;
	[SerializeField]
	public WeaponType _type;

	//this public property makes the field _type and takes action when it is set
	public WeaponType type {
		get {
			return(_type);
		}
		set {
			SetType (value);
		}
	}

	void Awake(){
		bndCheck = GetComponent<BoundsCheck> ();
		rend = GetComponent<Renderer> ();
		rigid = GetComponent<Rigidbody> ();
	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (bndCheck.offUp || bndCheck.offLeft || bndCheck.offRight) {
			Destroy (gameObject);
		}
	}

	public void SetType(WeaponType eType){
		//set the type 
		_type = eType;
		WeaponDefinition def = Main.GetWeaponDefinition (_type);
		rend.material.color = def.projectileColor;
	}
}
