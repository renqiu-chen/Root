using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubik : MonoBehaviour
{
    public GameObject[,,] Cubes=new GameObject[3,3,3];
    public bool moveAvailability;
    public bool moveLeft;
    public bool moveRight;
    public float moveTimeGap;
    private Rigidbody _rigidbody;
    [SerializeField] private Block currenBlock;
    [SerializeField] private Block targetBlock;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    Cubes[i, j, k] = transform.GetChild(i).GetChild(j).GetChild(k).gameObject;
                }
            }
        }
        _rigidbody = GetComponent<Rigidbody>();
        Reset();
    }

    public void Reset()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    Cubes[i, j, k].GetComponent<MeshRenderer>().enabled = false;
                }
            }
        }
        Cubes[1,1,1].GetComponent<MeshRenderer>().enabled = true;
    }

    
    private IEnumerator BetweenMove()
    {
        yield return new WaitForSeconds(moveTimeGap);
        moveAvailability = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveAvailability)
        {
            
        }
    }
}
