using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorTrigger : MonoBehaviour
{
    public GameObject particleEffect;
    public LegoBlock block;
    public void ParticaleSwitchOff()
    {
        particleEffect.SetActive(false);
        block.EnableCubes();
        Debug.Log("enabled");
    }

    public void ParticaleSwitchOn()
    {
        particleEffect.SetActive(true);

    }
}
