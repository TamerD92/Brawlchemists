using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Obstacle : MonoBehaviour
{

    public float FloorHeight;
    public float MinX;
    internal float MaxX;


    void Start()
    {
        Bounds bn = GetComponent<SpriteRenderer>().bounds;


        FloorHeight = bn.max.y;
        MinX = bn.min.x;
        MaxX = bn.max.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    internal bool IsInside(float positionX) {
        return positionX <= MaxX && positionX >= MinX;
    }
}
