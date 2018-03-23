using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour {
    [Header("Set in Inspector")]
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Set Dynamically")]
    public float camWidth;
    public float camHeight;
    public bool isOnScreen = true;

    [HideInInspector]
    public bool offRight, offLeft, offUp, offDown;

    void Start()
    {
        //get size of play screen from camera
        camHeight = Camera.main.orthographicSize;
        camWidth = camHeight * Camera.main.aspect;
    }



    void LateUpdate()
    { //Restricts ship from leaving play screen
        Vector3 pos = transform.position;
        isOnScreen = true;
        offRight = offLeft = offUp = offDown = false;

        if (pos.x > camWidth - radius) {
            pos.x = camWidth - radius;
            isOnScreen = false;
            offRight = true;
        }

        if (pos.x < -camWidth + radius) {
            pos.x = -camWidth + radius;
            isOnScreen = false;
            offLeft = true;
        }
        if (pos.y > camHeight - radius) {
            pos.y = camHeight - radius;
            isOnScreen = false;
            offUp = true;
        }
        if (pos.y < -camHeight + radius){
            pos.y = -camHeight + radius;
            isOnScreen = false;
            offDown = true;
        }

        isOnScreen = !(offRight || offLeft || offUp || offDown);
        if (keepOnScreen && !isOnScreen) {
            transform.position = pos;
            isOnScreen = true;
            offRight = offLeft = offUp = offDown = false;
        }
    }

    //draw bounds in scene pane 
    void OnDrawGizmos() {
        if (!Application.isPlaying) return;
        Vector3 boundSize = new Vector3(camWidth * 2, camHeight * 2, 0.1f);
        Gizmos.DrawWireCube(Vector3.zero, boundSize);
    }
}
