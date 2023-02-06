using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rubik : MonoBehaviour
{
    public GameObject[,,] Cubes=new GameObject[3,3,3];
    public bool moveAvailability;
    public bool moveLeft;
    public bool moveRight;
    public bool stopMoving;
    public bool blockFlag=false;
    public float moveTimeGap;
    private Rigidbody _rigidbody;
    public Vector3 velocity;
    private bool _moveForwardFlag=true;
    private bool _moveVerticalFlag=false;
    private bool _moveVerticalTimer = true;
    private bool _rotateTimer = true;
    private bool _rotateLeft=false;
    private bool _rotateRight=false;
    private bool _rotateUp=false;
    private bool _rotateDown=false;
    private Animator _animator;
    [SerializeField] private Block currentBlock;
    [SerializeField] private Block targetBlock;
    [SerializeField] private Block verticalTargetBlock;
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
        _animator = GetComponent<Animator>();
        moveAvailability = true;
        var ray = new Ray (transform.position, transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1f))
        {
            targetBlock = hit.transform.GetComponent<Block>();
        }
        Reset();
        // for (int i = 0; i < 3; i++)
        // {
        //     for (int j = 0; j < 3; j++)
        //     {
        //         for (int k = 0; k < 3; k++)
        //         {
        //             Debug.Log(Cubes[i,j,k]+Cubes[i,j,k].transform.parent.name+Cubes[i,j,k].transform.parent.parent.name);
        //         }
        //     }
        // }
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

            IEnumerator RotateMe(Vector3 dir,float inTime)
            {
                _rotateTimer = false;
                var fromAngle = transform.rotation;
                var toAngle = Quaternion.AngleAxis(90,dir);
                for(var t = 0f; t < 1; t += Time.deltaTime/inTime) {
                    transform.rotation = Quaternion.Lerp(fromAngle, toAngle, t);
                    yield return null;
                }
                transform.rotation = toAngle;
                
                _rotateTimer = true;
                yield return null;
            }
            
            
            if (!blockFlag)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    
                    if (_rotateTimer)
                    {
                        HorizontalRotate(true);
                        StartCoroutine(ResetRotateTimer(90,0,0));
                    }
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    
                    if (_rotateTimer)
                    {
                        HorizontalRotate(false);
                        StartCoroutine(ResetRotateTimer(-90,0,0));
                    }
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (_rotateTimer)
                    {
                        VerticalRotate(true);
                        StartCoroutine(ResetRotateTimer(0,0,-90));
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    
                    if (_rotateTimer)
                    {
                        VerticalRotate(false);
                        StartCoroutine(ResetRotateTimer(0,0,90));
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
    
    private IEnumerator ResetRotateTimer(float x,float y,float z)
    {
        _rotateTimer = false;
        transform.Rotate( x,y,z, Space.World);
        yield return new WaitForSeconds(0.25f);
        _rotateTimer = true;
        yield return null;
    }
    

    public void SetActiveCube(Vector3 input)
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

    public void Log()
    {
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                for (int k = 0; k < 3; k++)
                {
                    Debug.Log(Cubes[i, j, k].transform.name+Cubes[i, j, k].transform.parent.name+Cubes[i, j, k].transform.parent.parent.name);
                }
            }
        }
    }
    
    public void VerticalRotateTopTier(bool dir)
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
