using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegoBlock : Block
{
    public List<Vector3> activatedList;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rubik")
        {
            if (other.transform.position - this.transform.position == Vector3.zero)
            {
                currentRubik = other.GetComponent<Rubik>();
                MeshRenderer.material = stayMaterial;
                EnableCubes();
            }
            else
            {
                MeshRenderer.material = exitMaterial;
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
