using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBlock : Block
{
    public GameObject Rubic;

    public void OnEnable()
    {
        GameObject cube=Instantiate(Rubic,transform.position,Quaternion.identity);
        cube.transform.position = transform.position;
    }
}
