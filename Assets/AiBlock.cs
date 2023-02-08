using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiBlock : Block
{
    public GameObject RubicAi;

    public void OnEnable()
    {
        GameObject cube=Instantiate(RubicAi,transform.position,Quaternion.identity);
        cube.transform.position = transform.position;
    }
}
