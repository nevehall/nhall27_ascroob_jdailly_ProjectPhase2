using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy1 : Enemy{

	public float powerUpDropChance = 1f;

	public override void Move()
	{
		Vector3 tempPos = pos;
		tempPos.y -= speed * Time.deltaTime;

		if (angle == 3)
		{
			angle = Random.Range(0, 2);
			if (angle == 1) {
				tempPos.x -= speed * Time.deltaTime;
				pos = tempPos;
			}            else {
				tempPos.x += speed * Time.deltaTime;
				pos = tempPos;
			}
		}

		else {
			if (angle == 1)  {
				tempPos.x -= speed * Time.deltaTime;
				pos = tempPos;
			} else{
				tempPos.x += speed * Time.deltaTime;
				pos = tempPos;
			}
		}
	}

	public override void OnCollisionEnter(Collision coll)
	{
		GameObject otherGO = coll.gameObject;
		switch (otherGO.tag)
		{
		case "ProjectileHero":
			Projectile p = otherGO.GetComponent<Projectile>();
			//if this enemy is off screen, dont damage it
			if (!S._bndCheck.isOnScreen) {
				Destroy(otherGO);
				break;
			}

			//hurt this enemy
			S.ShowDamage();
			//get the damage amount from the main WEAP_DICT
			health -= Main.GetWeaponDefinition(p.type).damageOnHit;
			if (health <= 0) {
				Scores.AddPoints(score);
     				if (!S.notifiedOfDestruction) {
					Main.S.ShipDestroyed1 (this);
				}
				S.notifiedOfDestruction = true;
				//destroy this enemy & bullet
				Destroy(this.gameObject);
				Destroy(otherGO);
				break;
			}
			Destroy(otherGO);
			break;

		default:
			break;

		}
	}

	/*public override void DestroyAll(){
		base.DestroyAll ();
	}*/
}

