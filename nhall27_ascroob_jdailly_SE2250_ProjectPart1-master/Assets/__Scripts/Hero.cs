using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour {
    static public Hero S;

	public float gameRestartDelay = 5f;
	public GameObject projectilePrefab;
	public float projectileSpeed = 40;
	public Weapon[] weapons;
	public GameObject BombPrefab;


	private BoundsCheck _bndCheck;

	public Enemy E;


	[Header("These fields are set dynamically")]
	[SerializeField]

	private float _shieldLevel = 4;

	public int score;
	public Text scoreText;

	public bool invinc = false;
	public bool instantdeath=false;

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
	public Color[] originalColors;
	public Material[] materials; //all the materials of this & its children
	public bool showInvinc = false;

    public float camWidth;
    public float camHeight;


	float timeLeftDeath;
	float timeLeftInvinc;
	float timeTill = 2f;

	private float startTime;

	void Awake(){
		//get materials and colors for this GameObject and its children
		materials = Utils.GetAllMaterials(gameObject);
		originalColors = new Color[materials.Length];
		for (int i = 0; i < materials.Length; i++)
		{
			originalColors[i] = materials[i].color;
		}
	}

    void Start()
    {
        //get size of play screen from camera
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;

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


		timeLeftDeath -= Time.deltaTime;
		if (timeLeftDeath <= 0) {
			instantdeath = false;
		}
		if (timeLeftInvinc > 0 && showInvinc==false ) {
			foreach (Material m in materials)
			{
				m.color = Color.yellow;
			}

			showInvinc = true;
		}

		if (timeLeftInvinc <= 0) {
			for (int i = 0; i < materials.Length; i++)
			{
				materials[i].color = originalColors[i];
			}
			showInvinc = false;
		}

		timeLeftInvinc -= Time.deltaTime;
		if (timeLeftInvinc <= 0) {
			invinc = false;
		}
		if (timeTill > 0) {
			timeTill -= Time.deltaTime;
		}
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

		if (Input.GetKeyDown (KeyCode.B)) {
			weapons [0].SetType (WeaponType.blaster);
		}

		if (Input.GetKeyDown (KeyCode.X)) {
			weapons [0].SetType (WeaponType.single);
		}
		//reset the highscore
		if (Input.GetKeyDown (KeyCode.R)) {
			Scores.ResetHighScore ();
		}
			
		if(Input.GetKeyDown(KeyCode.K)){
			if(timeTill <=0){
				timeTill = 15f;
				GameObject[] gos = GameObject.FindGameObjectsWithTag ("Enemy");
				foreach (GameObject go in gos) {
					Destroy (go);
					BombDrop ();
				}
			}
		}


	}

	void BombDrop(){
		GameObject bomb = Instantiate<GameObject> (BombPrefab);
		var spawn = S.transform.position;
		bomb.transform.position = new Vector3 (spawn.x, spawn.y, 2f);

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
			if (invinc == false) {
				shieldLevel--;   //decrease shield level by 1
				Destroy (go);  //and destroy the enemy
			
			} else if (invinc == true) {
				Destroy (go);  //and destroy the enemys
				Scores.AddPoints(200);
			
			}
		} 
		else if (go.tag == "PowerUp") {
			//if the shield was triggered by a PowerUp
			AbsorbPowerUp (go);
		} else {
			print ("Triggered by non-Enemy: " + go.name);
		}
	}
	
	public void AbsorbPowerUp(GameObject go){
		PowerUp pu = go.GetComponent<PowerUp> ();

		switch(pu.powerUpType){
		case PowerUpType.invincible:
			print ("Invincibility");	//for testing purposes
			invinc = true;
			timeLeftInvinc = 5.0f;
			showInvinc = false;
			break;

		case PowerUpType.deathShot:
			print ("Death Shot");	//for testing purposes
			instantdeath = true;
			timeLeftDeath = 5.0f;
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
				Scores.Reset ();
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
