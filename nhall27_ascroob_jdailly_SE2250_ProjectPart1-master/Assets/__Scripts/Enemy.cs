using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
	[Header("Set in Unity Inspector")]
	public float speed; //  speed in m/s
	public float fireRate = 0.3f; //seconds/shot
	public float health = 10;
	public float showDamageDuration = 0.1f; //# seconds to show damage
	public int score ;
	public GameObject projectilePrefab;
	public float gunChance = 0.3f;
	public bool enemyShoot = true;

	protected float angle = 3;
	//public float powerUpDropChance; //chance to drop a power-up


	[Header("Set Dynamically: Enemy")]
	public Color[] originalColors;
	public Material[] materials; //all the materials of this & its children
	public bool showingDamage = false;
	public float damageDoneTime; //time to stop showing damage
	public bool notifiedOfDestruction = false; 

	public BoundsCheck _bndCheck;
	private Hero hero;
	public Enemy S;
	public bool shoot=false;
	public float num;

	void Start() {
		GameObject heroObject = GameObject.FindWithTag("Hero");
		if (heroObject != null) {
			hero = heroObject.GetComponent<Hero>();
		}
		if (hero == null) {
			Debug.Log("Cannot find 'Hero' script");
		}
	}

	void Awake(){
		_bndCheck = GetComponent<BoundsCheck>();
		//get materials and colors for this GameObject and its children
		materials = Utils.GetAllMaterials(gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++)
		{
			originalColors[i] = materials[i].color;
		}

		num = Random.value;
		if (Scores.score >= 3000 && shoot==false && enemyShoot && num < gunChance) {
			InvokeRepeating ("TempFire", fireRate,fireRate);
			shoot = true;
		}
	}

	public Vector3 pos {
		get {  return (this.transform.position); }
		set { this.transform.position = value; }
	}

	public virtual void Update(){
		Move();

		if (showingDamage && Time.time > damageDoneTime) { UnShowDamage(); }
		if (_bndCheck != null && (_bndCheck.offDown || _bndCheck.offLeft || _bndCheck.offRight)) {
			Destroy(gameObject);
			Destroy(GameObject.FindWithTag("Enemy"));
		}



	}

	public virtual void Move() {
		Vector3 tempPos = pos;
		tempPos.y -= speed * Time.deltaTime;
		pos = tempPos;
	}

	void TempFire(){
		GameObject bulletGO = Instantiate(projectilePrefab);
		bulletGO.tag = "Enemy";
		bulletGO.GetComponent<Renderer> ().material.color = Color.red;
		bulletGO.layer = 9;

		bulletGO.transform.position = transform.position;
		Rigidbody rB = bulletGO.GetComponent<Rigidbody>();
		//	rB.velocity = Vector3.up * projectileSpeed;

		Projectile proj = bulletGO.GetComponent<Projectile> ();
		proj.type = WeaponType.single;
		float tSpeed = Main.GetWeaponDefinition (proj.type).velocity;
		rB.velocity = Vector3.down * tSpeed;

		}


	public virtual void OnCollisionEnter(Collision coll) { }

	public void ShowDamage()
	{
		foreach (Material m in materials)
		{
			m.color = Color.red;
		}
		showingDamage = true;
		damageDoneTime = Time.time + showDamageDuration;
	}

	void UnShowDamage()
	{
		for (int i = 0; i < materials.Length; i++)
		{
			materials[i].color = originalColors[i];
		}
		showingDamage = false;
	}

}