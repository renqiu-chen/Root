using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RubikAi : Rubik
{
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
        moveAvailability = true;
        var ray = new Ray (transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            targetBlock = hit.transform.GetComponent<Block>();
        }
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
                    Cubes[i, j, k].SetActive(false);
                }
            }
        }
        Cubes[1,1,1].SetActive(true);
    }

    
    private IEnumerator BetweenMove()
    {
        yield return new WaitForSeconds(moveTimeGap);
        stopMoving = false;
        yield return null;
    }

    public void RefreshTarget()
    {
        stopMoving = true;
        _moveForwardFlag = true;
        var ray = new Ray (transform.position, currentBlock.blockDirection);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1.5f))
        {
            targetBlock = hit.transform.GetComponent<Block>();
        }
    }


    public void FixedUpdate()
    {
        if (moveAvailability)
        {
            if (!stopMoving)
            {
                this.transform.position =
                    Vector3.MoveTowards(this.transform.position, targetBlock.transform.position, 2*Time.fixedDeltaTime);
                if (_moveForwardFlag)
                {
                    currentBlock = targetBlock;
                    _moveForwardFlag = false;
                }
                if ((transform.position - targetBlock.transform.position).magnitude < 0.001f)
                {
                    RefreshTarget();
                    StartCoroutine(BetweenMove());
                }
            }
        }
    }

    public override void SetActiveCube(Vector3 input)
    {
        int i = (int)input.x;
        int j = (int)input.y;
        int k = (int)input.z;
        Cubes[i,j,k].SetActive(true);
    }

    public override void VerticalRotateTopTier(bool dir)
    {
        Vector3[,] positionList = new Vector3[3,3];
        GameObject[,,] changedList = new GameObject[3, 3, 3];
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                int x = k - 1;
                int y = j - 1;
                int x1, y1;
                if (dir)
                {
                    x1 = y + 1;
                    y1 = -x + 1;
                }
                else
                {
                    x1 = -y + 1;
                    y1 = x + 1;
                }
                positionList[y1,x1]=Cubes[2, j, k].transform.position;
                changedList[2, y1, x1] = Cubes[2, j, k];
            }
        }
        for (int j = 0; j < 3; j++)
        {
            for (int k = 0; k < 3; k++)
            {
                int x = k - 1;
                int y = j - 1;
                int x1, y1;
                if (dir)
                {
                    x1 = y + 1;
                    y1 = -x + 1;
                }
                else
                {
                    x1 = -y + 1;
                    y1 = x + 1;
                }
                Cubes[2, j, k].transform.position=positionList[j,k];
            }
        }
        Cubes = changedList;
    }

    public void Destroy()
    {
        Destroy(this);
    }
}
