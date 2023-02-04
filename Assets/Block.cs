using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    private MeshRenderer _meshRenderer;
    public Material stayMaterial;
    public Material exitMaterial;
    // Start is called before the first frame update
    void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rubik")
        {
            if (other.transform.position - this.transform.position == Vector3.zero)
            {
                _meshRenderer.material = stayMaterial;
            }
            else
            {
                _meshRenderer.material = exitMaterial;
            }
        }
    }
    
}
