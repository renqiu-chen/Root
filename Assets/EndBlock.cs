using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBlock : Block
{
    private bool _endFlag=true;
    public string nextScene;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rubik")
        {
            currentRubik = other.GetComponent<Rubik>();
            if (other.transform.position - this.transform.position == Vector3.zero)
            {
                currentRubik.blockFlag = true;
                MeshRenderer.material = stayMaterial;
                if (_endFlag)
                {
                    _endFlag = false;
                    SceneManager.LoadScene(nextScene);
                }
            }
            else
            {
                MeshRenderer.material = exitMaterial;
                currentRubik.blockFlag = false;
                _endFlag = true;
            }
        }
    }

}
