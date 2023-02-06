using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LegoBlock : Block
{
    public List<Vector3> activatedList;
    public List<GameObject> topCubes;
    public List<GameObject> rightCubes;
    public List<GameObject> leftCubes;
    public GameObject verticalMachine;
    public GameObject rightMachine;
    public GameObject leftMachine;
    private bool _legoFlag=true;

    public enum type
    {
        Vertical,
        Both
    };

    public type blockType;

    public void Start()
    {
        
        if (blockType==type.Vertical)
        {
            verticalMachine.SetActive(true);
            rightMachine.SetActive(false);
            leftMachine.SetActive(false);
            foreach (var cube in activatedList)
            {
                if (cube.x == 2)
                {
                    topCubes[(int)cube.y+3*(int)cube.z].SetActive(true);
                }
            }
        }
        if (blockType==type.Both)
        {
            verticalMachine.SetActive(false);
            rightMachine.SetActive(true);
            leftMachine.SetActive(true);
            foreach (var cube in activatedList)
            {
                if (cube.z == 0)
                {
                    leftCubes[(int)cube.y+3*(int)cube.x].SetActive(true);
                }
                if (cube.z == 2)
                {
                    rightCubes[(int)cube.y+3*(int)cube.x].SetActive(true);
                }
            }
        }
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
    public void OnEnable()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rubik")
        {
            currentRubik = other.GetComponent<Rubik>();
            if (other.transform.position - this.transform.position == Vector3.zero)
            {
                currentRubik.blockFlag = true;
                MeshRenderer.material = stayMaterial;
                if (_legoFlag)
                {
                    EnableCubes();
                    _legoFlag = false;
                }
            }
            else
            {
                MeshRenderer.material = exitMaterial;
                currentRubik.blockFlag = false;
                _legoFlag = true;
            }
        }
    }

    public void EnableCubes()
    {
        foreach (var input in activatedList)
        {
            currentRubik.SetActiveCube(input);
        }
    }

}
