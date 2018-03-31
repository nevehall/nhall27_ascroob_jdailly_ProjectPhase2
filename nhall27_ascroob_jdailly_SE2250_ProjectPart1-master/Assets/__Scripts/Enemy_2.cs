using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy {
    [Header("Set in Inspector: Enemy_2")]
    public float waveFreq = 2;
    public float waveWidth = 4;
    public float waveRotY = 45;

    private float x0; //inital x value of pos
    private float birthTime;

    void Start(){
        //set x0 to initial x pos of enemy
        x0 = pos.x;

        birthTime = Time.time;
    }

    public override void Move()
    {
        Vector3 tempPos = pos;

        float age = Time.time - birthTime;
        float theta = Mathf.PI * 2 * age / waveFreq;
        float sin = Mathf.Sin(theta);
        tempPos.x = x0 + waveWidth * sin;
        pos = tempPos;

        Vector3 rot = new Vector3(0, sin * waveRotY, 0);
        this.transform.rotation = Quaternion.Euler(rot);

        base.Move();
    }

    public override void OnCollisionEnter(Collision coll)
    {
        GameObject otherGO = coll.gameObject;
        switch (otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                //if this enemy is off screen, dont damage it
                if (!_bndCheck.isOnScreen)  {
                    Destroy(otherGO);
                    break;
                }

                //hurt this enemy
                S.ShowDamage();
                //get the damage amount from the main WEAP_DICT
                health -= Main.GetWeaponDefinition(p.type).damageOnHit;
                if (health <= 0)   {
                    Scores.AddPoints(score);
                    //destroy this enemy
					Destroy(this.gameObject);
                    Destroy(otherGO);
                    break;
                }
                Destroy(otherGO);
                break;

            default:
                break;

        }
        base.OnCollisionEnter(coll);
    }
}
