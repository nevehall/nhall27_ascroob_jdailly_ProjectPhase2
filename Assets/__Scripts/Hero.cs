using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
    static public Hero S;

    [Header("Set in Inspector")]
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;
    public float radius = 1f;

    [Header("Set Dynamically")]
    public float shieldLevel = 1;
    public float camWidth;
    public float camHeight;

    void Awake()
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
}
