using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    
    public void Start()
    {
        player = GameObject.Find("Rubik").transform;
    }

    // Update is called once per frame
    void Update () {
        transform.position = player.transform.position + offset;
    }
}
