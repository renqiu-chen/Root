using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckBlock :Block
{
    public bool[,] checkRightBools=new bool[3,3];
    public bool[,] checkBackBools=new bool[3,3];
    public List<Vector2> rightCheck;
    public List<Vector2> backCheck;
    public bool checkRight;
    public bool checkBack;
    public bool pass=true;
    private bool _checkFlag=true;
    public GameObject redlight;
    public GameObject greenlight;
    public AudioSource SuccessSound;
    public AudioSource FailSound;
    public List<GameObject> rightCheckCubes;
    public List<GameObject> backCheckCubes;


    public void OnEnable()
    {
        foreach (var right in rightCheck)
        {
            rightCheckCubes[(int)right.y+3*(int)right.x].SetActive(true);
        }
        foreach (var right in backCheck)
        {
            backCheckCubes[(int)right.y+3*(int)right.x].SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Rubik")
        {
            currentRubik = other.GetComponent<Rubik>();
            if ((other.transform.position - this.transform.position).magnitude<0.01f)
            {
                currentRubik.blockFlag = true;
                MeshRenderer.material = stayMaterial;
                if (_checkFlag)
                {
                    Check();
                    _checkFlag = false;
                }
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
                        Debug.Log(currentRubik.Cubes[i, j, k]);
                        if (currentRubik.Cubes[i, j, k].activeSelf)
                        {
                            if (!checkBackBools[i, k])
                            {
                                checkBackResult = false;
                                Debug.Log("back rubik:"+i+j+k);
                            }
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
                    Debug.Log("back check:"+back.x+back.y);
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
                        if (currentRubik.Cubes[i, j, k].activeSelf)
                        {
                            if (!checkRightBools[i, j])
                            {
                                checkRightResult = false;
                                Debug.Log("right rubik:"+i+j+k);
                            }
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
                    Debug.Log("right check:"+right.x+right.y);
                }
            }
            

            if (!checkRightResult)
            {
                pass = false;
            }
        }

        if (pass)
        {
            greenlight.SetActive(true);
            SuccessSound.Play();
        }
        else
        {
            redlight.SetActive(true);
            currentRubik.moveAvailability = false;
            FailSound.Play();
            if (currentRubik.GetType() == typeof(RubikAi))
            {
                currentRubik.SelfDestroy();
            }
            else
            {
                currentRubik.SelfDestroy();
                StartCoroutine(RestartGame());
            }
            
        }
    }

    private IEnumerator RestartGame()
    {
        yield return new WaitForSeconds(1f);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        yield return null;
    }

    
}
