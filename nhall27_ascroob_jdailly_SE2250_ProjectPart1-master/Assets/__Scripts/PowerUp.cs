using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType{
	none,	//default; no power up 
	invincible,	//invincibility for the hero
	deathShot	//kills the enemy with one shot
}

public class PowerUp : MonoBehaviour {
	[Header("Set in Inspector")]
	//unusual but hand use of Vector2s
	//x holds a min value and y a max value for a Random.Range() 
	public Vector2 rotMinMax = new Vector2(15,90);
	public Vector2 driftMinMax = new Vector2(.25f,2);
	public float lifeTime = 6f; //seconds the PowerUp exists
	public float fadeTime = 4f; //seconds it will then fade

	[Header("Set Dynamically")]
	public WeaponType type; //the type of the PowerUp
	public PowerUpType powerUpType = PowerUpType.none; //the type of PowerUp to drop
	public GameObject cube; //reference to the Cube child
	public TextMesh letter; //reference to the TextMesh
	public Vector3 rotPerSecond; //euler rotation speed
	public float birthTime; 

	private Rigidbody rigid;
	private BoundsCheck bndCheck;
	private Renderer cubeRend;

	void Awake(){
		//find the cube reference
		cube = transform.Find("Cube").gameObject;
		//find the TextMesh and other components
		letter = GetComponent<TextMesh>();
		rigid = GetComponent<Rigidbody> ();
		bndCheck = GetComponent<BoundsCheck> ();
		cubeRend = cube.GetComponent<Renderer> ();

		//set a random velocity
		Vector3 vel = Random.onUnitSphere; //get random XYZ velocity
		//random.onUnitSphere gives you a vector point that is somewhere on the 
		//surface of the sphere with a radius of 1m around the origin 
		vel.z = 0; //flatten the vel to the XY plane
		vel.Normalize(); //normalizing a Vector3 makes it length 1m
		vel *= Random.Range(driftMinMax.x, driftMinMax.y);
		rigid.velocity = vel;

		//set the rotation of this GameObject to R:[0,0,0]
		transform.rotation = Quaternion.identity;
		//quaternion.identity is equal to no rotation

		//set up the rotPerSecond for the Cube child using rotMinMaxx & y
		rotPerSecond = new Vector3 (Random.Range(rotMinMax.x, rotMinMax.y),
			Random.Range(rotMinMax.x,rotMinMax.y),
			Random.Range(rotMinMax.x, rotMinMax.y));
		
		birthTime = Time.time;


	}


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		cube.transform.rotation = Quaternion.Euler (rotPerSecond * Time.time);

		//fade out the powerUp over time
		//given the default values, a PowerUp will exist for 10 seconds
		//and then fade out over 4 seconds
		float u = (Time.time - (birthTime + lifeTime)) / fadeTime;
		//for lifeTime seconds, u will be <= 0. then it will transition to 
		// 1 over the course of fadeTime seconds

		//if u >= 1, destroy this PowerUp
		if (u >= 1) {
			Destroy (this.gameObject);
			return;
		}

		//use u to determine the alpha value of the Cube & Letter
		if (u > 0) {
			Color c = cubeRend.material.color;
			c.a = 1f - u;
			cubeRend.material.color = c;
			//fade the letter too, just not as much
			c = letter.color;
			c.a = 1f - (u * 0.5f);
			letter.color = c;
		}

		if (!bndCheck.isOnScreen) {
			//if the PowerUp has drifted entirely off the screen, destroy it
			Destroy (gameObject);
		}
	}

	public void SetType(WeaponType wt){
		//grab the WeaponDefinition from the Main
		WeaponDefinition def = Main.GetWeaponDefinition(wt);
		//set the color of the Cube chid
		cubeRend.material.color = def.color;
		letter.text = def.letter; //set the letter that is shown
		type = wt; //finally actually set the type
	}

	public void AbsorbedBy(GameObject target){
		//this function is called by the Hero class when a PowerUp is collected
		//we could tween into the target and shrink in size,
		//but for now, just destroy this.gameObject
		Destroy (this.gameObject);
	}

	/*public void SetPowerUpType(PowerUpType pt){
		switch (powerUpType) {
		case PowerUpType.invincible:
			print ("working invincibility");
			break;
		case PowerUpType.deathShot:
			print ("working death Shot");
			break;

		}
	}*/
}
