﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    public class Enemies : MonoBehaviour {
    public float speed; //  speed in m/s

    private BoundsCheck _bndCheck;

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
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

        if (_bndCheck != null && _bndCheck.offDown) {
                Destroy(gameObject);
            }
        }

     public virtual void Move()  {
            Vector3 tempPos = pos;
            tempPos.y -= speed * Time.deltaTime;
            pos = tempPos;
        }
    }

