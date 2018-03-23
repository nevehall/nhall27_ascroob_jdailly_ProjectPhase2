using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : MonoBehaviour {
    public float speed; //  speed in m/s

    protected float angle = 3;

    private BoundsCheck _bndCheck;

    void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
    }

    public Vector3 pos
    {
        get
        {
            return (this.transform.position);
        }
        set
        {
            this.transform.position = value;
        }
    }

    void Update()
    {
        Move();

        if (_bndCheck != null && _bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }

    public virtual void Move()
    {
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;

        if (angle == 3)
        {
            angle = Random.Range(0, 2);
            if (angle == 1)
            {
                tempPos.x -= speed * Time.deltaTime;
                pos = tempPos;
            }
            else
            {
                tempPos.x += speed * Time.deltaTime;
                pos = tempPos;
            }
        }

        else
        {
            if (angle == 1)
            {
                tempPos.x -= speed * Time.deltaTime;
                pos = tempPos;
            }
            else
            {
                tempPos.x += speed * Time.deltaTime;
                pos = tempPos;
            }
        }

    }
}

