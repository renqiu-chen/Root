using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RotateBlock : Block
{
    public List<Vector3> activatedList;
    private bool _rotateFlag=true;
    private bool _enableRotate;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rubik")
        {
            currentRubik = other.GetComponent<Rubik>();
            if (other.transform.position - this.transform.position == Vector3.zero)
            {
                if (_rotateFlag)
                {
                    _rotateFlag = false;
                    currentRubik.blockFlag = true;
                }
                MeshRenderer.material = stayMaterial;
            }
            else
            {
                MeshRenderer.material = exitMaterial;
                currentRubik.blockFlag = false;
                _rotateFlag = true;
            }
        }
    }

    public void RotateCubesCounterClockWise()
    {
        if (!_rotateFlag)
        {
            currentRubik.VerticalRotateTopTier(true);
        }
    }

    public void RotateCubesClockWise()
    {
        if (!_rotateFlag)
        {
            currentRubik.VerticalRotateTopTier(false);
        }
    }
}
