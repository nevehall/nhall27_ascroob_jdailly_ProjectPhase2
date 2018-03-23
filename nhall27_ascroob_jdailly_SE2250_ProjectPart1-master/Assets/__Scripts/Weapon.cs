﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this is an enum of the various possible weapon types

public enum WeaponType{
	none, //default; no weapon
	blaster, //3 shots, one in middle the other 2 at 30 degree angles right and left
	single, //singl shots 
	phaser, //shots that move in waves
	missile, //homing missiles
	laser, //damage over time
	shield //raise shield level
}

//the weapon definition class allows you to set the properties
//of a specific weapon in the Inspector. The main class has an array 
//of WeaponDefinitions that makes this possible
[System.Serializable]
public class WeaponDefinition{
	public WeaponType type = WeaponType.none;
	public string letter; //letter to show on power up 
	public Color color = Color.white;   //color of collar & power up  
	public GameObject projectilePrefab;    //prefab for projectile
	public Color projectileColor = Color.white;
	public float damageOnHit = 0;  //amount of damage on caused
	public float continuousDamage = 0; //damage per second (for the laser)
	public float delayBetweenShots = 0;  //phaser ?
	public float velocity = 20; //speed for projectiles
}

public class Weapon : MonoBehaviour {
	static public Transform PROJECTILE_ANCHOR;

	[Header("Set Dynamically")] [SerializeField]
	private WeaponType _type = WeaponType.none;
	public WeaponDefinition def;
	public GameObject collar;
	public float lastShotTime; //time last shot was fired
	private Renderer collarRend;

	// Use this for initialization
	void Start () {
		collar = transform.Find("Collar").gameObject;
		collarRend = collar.GetComponent<Renderer> ();

		//call SetType() for the default _type of the WeaponType.none
		SetType(_type);

		//dynamically create an anchor for all projectiles
		if (PROJECTILE_ANCHOR == null) {
			GameObject go = new GameObject ("_ProjectileAnchor");
			PROJECTILE_ANCHOR = go.transform;
		}

		//find the fireDelegate of the root GameObject
		GameObject rootGO = transform.root.gameObject;
		if (rootGO.GetComponent<Hero> () != null) {
			rootGO.GetComponent<Hero> ().fireDelegate += Fire;
		}
				
	}

	public WeaponType type{
		get{ return(_type); }
		set{ SetType (value); }
	}

	public void SetType(WeaponType wt){
		_type = wt;
		if (type == WeaponType.none) {
			this.gameObject.SetActive (false);
			return;
		} else {
			this.gameObject.SetActive (true);
		}
		def = Main.GetWeaponDefinition (_type);
		collarRend.material.color = def.color;
		lastShotTime = 0; //you can fire immediately after _type is set
	}

	public void Fire(){
		//if this.gameObject is inactive, return
		if(!gameObject.activeInHierarchy) return;
		//if it hasn't been enough time between shots, return
		if (Time.time - lastShotTime < def.delayBetweenShots) {
			return;
		}
		Projectile p;
		Vector3 vel = Vector3.up * def.velocity;
		if (transform.up.y < 0) {
			vel.y = -vel.y;
		}

		switch (type) {
		case WeaponType.blaster:
			p = MakeProjectile (); //make middle projectile
			p.rigid.velocity = vel;
			p = MakeProjectile ();  //make right projectile
			p.transform.rotation = Quaternion.AngleAxis (10, Vector3.back);
			p.rigid.velocity = p.transform.rotation * vel;
			p = MakeProjectile ();  //make left projectile
			p.transform.rotation = Quaternion.AngleAxis (-10, Vector3.back);
			p.rigid.velocity = p.transform.rotation * vel;
			break;
		case WeaponType.single:
			p = MakeProjectile ();
			p.rigid.velocity = vel;
			break;

		}
	}

	public Projectile MakeProjectile(){
		GameObject go = Instantiate<GameObject> (def.projectilePrefab);
		if (transform.parent.gameObject.tag == "Hero") {
			go.tag = "ProjectileHero";
			go.layer = LayerMask.NameToLayer ("ProjectileHero");
		} else {
			go.tag = "ProjectileEnemy";
			go.layer = LayerMask.NameToLayer ("ProjectileEnemy");
		}
		go.transform.position = collar.transform.position;
		go.transform.SetParent (PROJECTILE_ANCHOR, true);
		Projectile p = go.GetComponent<Projectile> ();
		p.type = type;
		lastShotTime = Time.time;
		return(p);
	}
	// Update is called once per frame
	void Update () {
		
	}
}
