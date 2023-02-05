using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateBlock : Block
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
            }
            else
            {
                MeshRenderer.material = exitMaterial;
            }
        }
    }

    public void RotateCubes()
    {
        currentRubik.VerticalRotateTopTier(true);
    }
}
