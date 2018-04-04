using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_0: Enemy { 

	public float powerUpDropChance = 1f;

	public override void Move() {
		base.Move();
	}

	public override void Update(){
		base.Update ();
	}

	public override void OnCollisionEnter(Collision coll) { 
		GameObject otherGO = coll.gameObject;
		switch (otherGO.tag)   {
		case "ProjectileHero":
			Projectile p = otherGO.GetComponent<Projectile> ();
			//if this enemy is off screen, dont damage it
			if (!_bndCheck.isOnScreen) {
				Destroy (otherGO);
				break;
			}
			if (Hero.S.instantdeath == true) {
				S.ShowDamage ();
				Destroy (this.gameObject);
				Scores.AddPoints (score);
			}
			//hurt this enemy
			S.ShowDamage ();
			//get the damage amount from the main WEAP_DICT
			health -= Main.GetWeaponDefinition (p.type).damageOnHit;
			if (health <= 0) {
				Scores.AddPoints (score);
				if (!S.notifiedOfDestruction) {
					Main.S.ShipDestroyed0 (this);
				}
				S.notifiedOfDestruction = true;
				//destroy this enemy
				Destroy (this.gameObject);
				Destroy (otherGO);
				break;
			}
			Destroy(otherGO);
			break;

		default:
			print("Enemy hit by non-ProjectileHero: " + otherGO.name);
			break;

		}
	}
		
}
