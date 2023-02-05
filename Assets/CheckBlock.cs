using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckBlock :Block
{
    public List<Vector3> topCheck;
    public List<Vector3> rightCheck;
    public List<Vector3> backCheck;
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
