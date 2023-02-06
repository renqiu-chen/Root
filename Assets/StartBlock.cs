using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBlock : Block
{
    public GameObject Rubic;

    public void OnEnable()
    {
        Instantiate(Rubic,transform.position,Quaternion.identity);
    }
}
