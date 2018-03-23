﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemies : MonoBehaviour {

	[Header("Set in Unity Inspector")]
    public float speed; //  speed in m/s
	public float fireRate = 0.3f; //seconds/shot
	public float health = 10; 
	public int score = 100; //points earned for destroying this
	public float showDamageDuration = 0.1f; //# seconds to show damage

	[Header("Set Dynamically: Enemy")]
	public Color[] originalColors;
	public Material[] materials; //all the materials of this & its children
	public bool showingDamage = false;
	public float damageDoneTime; //time to stop showing damage
	public bool notifiedOfDestruction = false; //will be used later

    private BoundsCheck _bndCheck;

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
		//get materials and colors for this GameObject and its children
		materials = Utils.GetAllMaterials(gameObject);
		originalColors = new Color[materials.Length];
			for(int i =0; i<materials.Length; i++){
				originalColors[i] = materials[i].color;
			}
    }

    public Vector3 pos { 
         get {
                return (this.transform.position);
            }
         set {
                this.transform.position = value;
            }
        }

    void Update() {
        Move();

		if(showingDamage && Time.time > damageDoneTime){
			UnShowDamage ();
		}

        if (_bndCheck != null && _bndCheck.offDown) {
                Destroy(gameObject);
            }
        }

     public virtual void Move()  {
            Vector3 tempPos = pos;
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }

	void OnCollisionEnter( Collision coll ) {
		GameObject otherGO = coll.gameObject;       
		switch (otherGO.tag){
		case "ProjectileHero":
			Projectile p = otherGO.GetComponent<Projectile>();
			//if this enemy is off screen, dont damage it
			if(!_bndCheck.isOnScreen){
				Destroy(otherGO);
				break;
			}

			//hurt this enemy
			ShowDamage();
			//get the damage amount from the main WEAP_DICT
			health -= Main.GetWeaponDefinition(p.type).damageOnHit;
			if(health<= 0){
				//destroy this enemy
				Destroy(this.gameObject);
			}
			Destroy(otherGO);
			break;

		default:
			print("Enemy hit by non-ProjectileHero: " +otherGO.name);
			break;
		
     	}
	}

	void ShowDamage(){
		foreach (Material m in materials) {
			m.color = Color.red;
		}
		showingDamage = true;
		damageDoneTime = Time.time + showDamageDuration;
	}

	void UnShowDamage(){
		for (int i = 0; i < materials.Length; i++) {
			materials [i].color = originalColors [i];
		}
		showingDamage = false;
	}
}
