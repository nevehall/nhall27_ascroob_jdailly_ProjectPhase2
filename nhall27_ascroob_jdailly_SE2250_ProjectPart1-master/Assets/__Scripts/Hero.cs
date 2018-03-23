﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    static public Hero S;

	public float gameRestartDelay = 2f;
	public GameObject projectilePrefab;
	public float projectileSpeed = 40;
	public Weapon[] weapons;

	[Header("These fields are set dynamically")]
	[SerializeField]

	private float _shieldLevel = 4;

	//this variable holds a reference to the last triggering GameObject
	private GameObject lastTriggerGo = null;

	//declare a new delegate type WeaponFireDelegate
	public delegate void WeaponFireDelegate();
	//create a weaponFireDelegate field named fireDelegate
	public WeaponFireDelegate fireDelegate;

    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float radius = 1f;

    [Header("Set Dynamically")]

    public float camWidth;
    public float camHeight;

    void Start()
    {
        //get size of play screen from camera
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;

		//fireDelegate += TempFire;

        if (S == null)
        {
            S = this; //Set the Singleton
        } else
        {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!");
        }

		//reset the weapons to start _Hero with single
		ClearWeapons();
		weapons[0].SetType (WeaponType.single);

    }

	// Update is called once per frame
	void Update () {
        //Pull in info from Input class
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        //Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // Rotate the ship to make it feel more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);

		//use the fireDelegate to fire Weapons
		//first, make sure the button is pressed Axis("Jump")
		//then ensure that fireDelegate isn't null to avoid an error
		if (Input.GetAxis ("Jump") == 1 && fireDelegate != null) {
			fireDelegate ();
		}
	}

    void LateUpdate()
    { //Restricts ship from leaving play screen
        Vector3 pos = transform.position;

        if (pos.x > camWidth - radius)  {
            pos.x = camWidth - radius;
        }

        if (pos.x < -camWidth + radius) {
            pos.x = -camWidth + radius;
        }
        if (pos.y > camHeight - radius){
            pos.y = camHeight - radius;
        }
        if (pos.y < -camHeight + radius)  {
            pos.y = -camHeight + radius;
        }

        transform.position = pos;
    }

    //draw bounds in scene pane 
    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }

	void OnTriggerEnter(Collider other) {
		Transform rootT = other.gameObject.transform.root;
		GameObject go = rootT.gameObject;
		print ("Triggered: " + go.name);

		if (go == lastTriggerGo) {
			return;
		}
		lastTriggerGo = go;

		//if shield was triggered by an enemy
		if (go.tag == "Enemy") {
			shieldLevel--;   //decrease shield level by 1
			Destroy (go);  //and destroy the enemy
		} else if (go.tag == "PowerUp") {
			//if the shield was triggered by a PowerUp
			AbsorbPowerUp (go);
		} else {
			print ("Triggered by non-Enemy: " + go.name);
		}
	}
	
	public void AbsorbPowerUp(GameObject go){
		PowerUp pu = go.GetComponent<PowerUp> ();
		switch (pu.type) {
			case WeaponType.shield:
				shieldLevel++;
				break;

		default:
			if (pu.type == weapons [0].type) { //if it is the same type
				Weapon w = GetEmptyWeaponSlot ();
				if (w != null) {
					//set it to pu.type
					w.SetType (pu.type);
				}
			} else {
				ClearWeapons ();
				weapons [0].SetType (pu.type);
			}
			break;

		}
		pu.AbsorbedBy (this.gameObject);
	}

	public float shieldLevel {
		get {
			return(_shieldLevel);
		}
		set { 
			_shieldLevel = Mathf.Min (value, 4);
			if (value < 0) {
				Destroy(this.gameObject);
				//tell Main.S to restart the game after a delay
				Main.S.DelayedRestart(gameRestartDelay);
			}
		}
	}

	Weapon GetEmptyWeaponSlot() {
		for (int i = 0; i < weapons.Length; i++) {
			if (weapons [i].type == WeaponType.none) {
				return(weapons [i]);
			}
		}
		return(null);
	}

	void ClearWeapons(){
		foreach (Weapon w in weapons) {
			w.SetType (WeaponType.none);
		}
	}

}
