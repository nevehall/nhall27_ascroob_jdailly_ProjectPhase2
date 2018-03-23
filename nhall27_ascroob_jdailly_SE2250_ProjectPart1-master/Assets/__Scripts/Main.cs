
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour {
    static public Main S; //singleton for main

	static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT; 

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;

	public WeaponDefinition[] weaponDefinitions;

    private BoundsCheck _bndCheck;

    void Awake()
    {
        S = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);

		//a generic dictionary with WeaponType as the key
		WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>(); 
		foreach(WeaponDefinition def in weaponDefinitions){
			WEAP_DICT[def.type] = def;
		}
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);

        //position enemy above screen with random x position
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
        {
            enemyPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);
        }
        //set initial position for spawned enemy
        Vector3 pos = Vector3.zero;
        float xMin = -_bndCheck.camWidth + enemyPadding;
        float xMax = _bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = _bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;

        //invoke spawnEnemy() again
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond);
    }

	public void DelayedRestart(float delay){
		//invoke the restart method in delay seconds
		Invoke ("Restart", delay);
	}

	public void Restart(){
		//reload _scene_0 to restart the game
		SceneManager.LoadScene("_Scenes/Main");
	}

	//static function that gets a WeaponDefinition from the WEAP_DICT static 
	//protected field of the Main class
	//returns the WeaponDefinition or, if there is no WeaponDefinition
	//with the WeaponType passed in, returns a new WeaponDefinition with a 
	//WeaponType of none

	static public WeaponDefinition GetWeaponDefinition(WeaponType wt){
		if (WEAP_DICT.ContainsKey (wt)) {
			return(WEAP_DICT [wt]);
		}
		return (new WeaponDefinition ());
	}

}
