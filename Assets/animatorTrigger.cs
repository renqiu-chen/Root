using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorTrigger : MonoBehaviour
{
    public GameObject particleEffect;
    public bool isLego;
    public LegoBlock block;
    public void ParticaleSwitchOff()
    {
        particleEffect.SetActive(false);
        if (isLego)
        {
            block.EnableCubes();
        }
    }

    public void ParticaleSwitchOn()
    {
        particleEffect.SetActive(true);
    }
}
