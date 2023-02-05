using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckBlock :Block
{
    public bool[,] checkRightBools=new bool[3,3];
    public bool[,] checkBackBools=new bool[3,3];
    public List<Vector2> rightCheck;
    public List<Vector2> backCheck;
    public bool checkRight;
    public bool checkBack;
    public bool pass=true;
    private bool _checkFlag;
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rubik")
        {
            currentRubik = other.GetComponent<Rubik>();
            if (other.transform.position - this.transform.position == Vector3.zero)
            {
                MeshRenderer.material = stayMaterial;
                if (_checkFlag)
                {
                    Check();
                    _checkFlag = false;
                }
                currentRubik.blockFlag = true;
            }
            else
            {
                MeshRenderer.material = exitMaterial;
                currentRubik.blockFlag = false;
                _checkFlag = true;
            }
        }
    }

    public void Check()
    {
        foreach (var flag in rightCheck)
        {
            checkRightBools[(int)flag.x, (int)flag.y] = true;
        }
        foreach (var flag in backCheck)
        {
            checkBackBools[(int)flag.x, (int)flag.y] = true;
        }
        if (checkBack)
        {
            bool checkBackResult = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (currentRubik.Cubes[i, j, k].activeSelf && !checkBackBools[i, k])
                        {
                            checkBackResult = false;
                        }
                    }
                }
            }

            foreach (var back in backCheck)
            {
                if (!(currentRubik.Cubes[(int)back.x, 0, (int)back.y].activeSelf ||
                      currentRubik.Cubes[(int)back.x, 1, (int)back.y].activeSelf ||
                      currentRubik.Cubes[(int)back.x, 2, (int)back.y].activeSelf))
                {
                    checkBackResult = false;
                }
            }
            

            if (!checkBackResult)
            {
                pass = false;
            }
        }

        if (checkRight)
        {
            bool checkRightResult = true;
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    for (int k = 0; k < 3; k++)
                    {
                        if (currentRubik.Cubes[i, j, k].activeSelf && !checkRightBools[i, j])
                        {
                            checkRightResult = false;
                            Debug.Log("rubik more");
                        }
                    }
                }
            }

            foreach (var right in rightCheck)
            {
                if (!(currentRubik.Cubes[(int)right.x, (int)right.y, 0].activeSelf ||
                      currentRubik.Cubes[(int)right.x, (int)right.y, 1].activeSelf ||
                      currentRubik.Cubes[(int)right.x, (int)right.y, 2].activeSelf))
                {
                    checkRightResult = false;
                    Debug.Log("check more");
                }
            }
            

            if (!checkRightResult)
            {
                pass = false;
            }
        }

        if (pass)
        {
            Debug.Log("pass");
        }
        else
        {
            Debug.Log("not pass");
        }
    }
}