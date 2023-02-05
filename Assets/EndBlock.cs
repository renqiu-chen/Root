using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBlock : Block
{
    private bool _endFlag=true;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rubik")
        {
            currentRubik = other.GetComponent<Rubik>();
            if (other.transform.position - this.transform.position == Vector3.zero)
            {
                currentRubik.moveAvailability = false;
                MeshRenderer.material = stayMaterial;
                if (_endFlag)
                {
                    _endFlag = false;
                    Scene scene = SceneManager.GetActiveScene();
                    SceneManager.LoadScene(scene.buildIndex+1);
                }
            }
            else
            {
                MeshRenderer.material = exitMaterial;
                _endFlag = true;
            }
        }
    }

}
