using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    protected MeshRenderer MeshRenderer;
    public Material stayMaterial;
    public Material exitMaterial;
    public Rubik currentRubik;

    public Block leftBlock;
    public Block rightBlock;
    public Vector3 blockDirection;
    // Start is called before the first frame update
    void Start()
    {
        blockDirection = transform.forward;
        MeshRenderer = GetComponent<MeshRenderer>();
        var rayLeft = new Ray (transform.position, -transform.right);
        RaycastHit hitLeft;
        if (Physics.Raycast(rayLeft, out hitLeft, 1f))
        {
            leftBlock = hitLeft.transform.GetComponent<Block>();
        }
        MeshRenderer = GetComponent<MeshRenderer>();
        var ray = new Ray (transform.position, transform.right);
        RaycastHit hitright;
        if (Physics.Raycast(ray, out hitright, 1f))
        {
            rightBlock = hitright.transform.GetComponent<Block>();
        }
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
                currentRubik = other.GetComponent<Rubik>();
                MeshRenderer.material = stayMaterial;
            }
            else
            {
                MeshRenderer.material = exitMaterial;
            }
        }
    }
    
}
