using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animatorTrigger : MonoBehaviour
{
    public GameObject particleEffect;
    void ParticaleSwitchOff()
    {
        particleEffect.SetActive(false);
    }

    void ParticaleSwitchOn()
    {
        particleEffect.SetActive(true);
    }
}
