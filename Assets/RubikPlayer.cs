using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubikPlayer : Rubik
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
                    Vector3.MoveTowards(this.transform.position, targetBlock.transform.position, moveSpeed*Time.fixedDeltaTime);
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

            if (!rotateAbility)
            {
                transform.Rotate(rotateAngle*Time.fixedDeltaTime*4,Space.World);
            }
            if (stopMoving)
            {
                if (blockFlag)
                {
                    return;
                }
                if (Input.GetKey(KeyCode.A))
                {
                    if (targetBlock.leftBlock)
                    {
                        if (_moveVerticalTimer)
                        {
                            verticalTargetBlock = currentBlock.leftBlock;
                            currentBlock = currentBlock.leftBlock;
                            _moveVerticalFlag = true;
                            _moveVerticalTimer = false;
                            StartCoroutine(ResetVerticalTimer());
                        }
                    }

                }
                if (Input.GetKey(KeyCode.D))
                {
                    if (targetBlock.rightBlock)
                    {
                        if (_moveVerticalTimer)
                        {
                            verticalTargetBlock = currentBlock.rightBlock;
                            currentBlock = currentBlock.rightBlock;
                            _moveVerticalFlag = true; 
                            _moveVerticalTimer = false;
                            StartCoroutine(ResetVerticalTimer());
                        }
                    }
                }
                if (_moveVerticalFlag)
                {
                    this.transform.position =
                        Vector3.MoveTowards(this.transform.position, verticalTargetBlock.transform.position, 50*Time.fixedDeltaTime);
                    
                    if ((transform.position - verticalTargetBlock.transform.position).magnitude < 0.001f)
                    {
                        _moveVerticalFlag = false;
                        RefreshTarget();
                    }
                }
            }
            
            
            
            if (!blockFlag)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (rotateAbility)
                    {
                        HorizontalRotate(true);
                        StartCoroutine(Rotate(90,0,0));
                    }
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (rotateAbility)
                    {
                        HorizontalRotate(false);
                        StartCoroutine(Rotate(-90,0,0));
                    }
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (rotateAbility)
                    {
                        VerticalRotate(true);
                        StartCoroutine(Rotate(0,0,-90));
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    
                    if (rotateAbility)
                    {
                        VerticalRotate(false);
                        
                        StartCoroutine(Rotate(0,0,90));
                    }
                }
            }
        }
    }
    


    private IEnumerator ResetVerticalTimer()
    {
        yield return new WaitForSeconds(0.25f);
        _moveVerticalTimer = true;
        yield return null;
    }
    
    private IEnumerator Rotate(float x,float y,float z)
    {
        rotateAbility = false;
        rotateAngle.x = x;
        rotateAngle.y = y;
        rotateAngle.z = z;
        _currentQuaternion = transform.rotation;
        Quaternion eulerRot = Quaternion.Euler(x, y, z);
        _targetQuaternion =
            _currentQuaternion * (Quaternion.Inverse(_currentQuaternion) * eulerRot * _currentQuaternion);
        yield return new WaitForSeconds(0.25f);
        rotateAngle.x = 0;
        rotateAngle.y = 0;
        rotateAngle.z = 0;
        transform.rotation = _targetQuaternion;
        rotateAbility = true;
    }
    

    public override void SetActiveCube(Vector3 input)
    {
        int i = (int)input.x;
        int j = (int)input.y;
        int k = (int)input.z;
        Cubes[i,j,k].SetActive(true);
    }

    public void VerticalRotate(bool dir)
    {
        GameObject[,,] changedList = new GameObject[3, 3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    int x = k - 1;
                    int y = i - 1;
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
                    changedList[y1,j,x1] = Cubes[i, j, k];
                }
            }
        }
        Cubes = changedList;
    }
    
    public void Hello(){}
    
    public void HorizontalRotate(bool dir)
    {
        GameObject[,,] changedList = new GameObject[3, 3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    int x = j - 1;
                    int y = i - 1;
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
                    changedList[y1,x1,k] = Cubes[i, j, k];
                }
            }
        }
        Cubes = changedList;
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
}
