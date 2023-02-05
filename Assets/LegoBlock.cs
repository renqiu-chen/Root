using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LegoBlock : Block
{
    public List<Vector3> activatedList;
    private bool _legoFlag=true;
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
