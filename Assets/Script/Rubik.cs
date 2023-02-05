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
                    Vector3.MoveTowards(this.transform.position, targetBlock.transform.position, Time.fixedDeltaTime);
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

            // if (_rotateLeft)
            // {
            //     Vector3 to = new Vector3(0, -90, 0);
            //     if (Vector3.Distance(transform.eulerAngles, to) > 0.01f)
            //     {
            //         transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
            //     }
            //     else
            //     {
            //         transform.eulerAngles = to;
            //         _rotateLeft = false;
            //         _rotateTimer = true;
            //     }
            // }
            //
            // if (_rotateRight)
            // {
            //     Vector3 to = new Vector3(0, 90, 0);
            //     if (Vector3.Distance(transform.eulerAngles, to) > 0.01f)
            //     {
            //         transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, 0.5f*Time.deltaTime);
            //     }
            //     else
            //     {
            //         transform.eulerAngles = to;
            //         _rotateRight = false;
            //         _rotateTimer = true;
            //     }
            // }
            //
            // if (_rotateUp)
            // {
            //     Vector3 to = new Vector3(0, 0, 90);
            //     if (Vector3.Distance(transform.eulerAngles, to) > 0.01f)
            //     {
            //         transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, 0.5f*Time.deltaTime);
            //     }
            //     else
            //     {
            //         transform.eulerAngles = to;
            //         _rotateUp = false;
            //         _rotateTimer = true;
            //     }
            // }
            //
            // if (_rotateDown)
            // {
            //     Vector3 to = new Vector3(0, 0, -90);
            //     if (Vector3.Distance(transform.eulerAngles, to) > 0.01f)
            //     {
            //         transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, 0.5f*Time.deltaTime);
            //     }
            //     else
            //     {
            //         transform.eulerAngles = to;
            //         _rotateDown = false;
            //         _rotateTimer = true;
            //     }
            // }
            
            // Vector3 a = new Vector3(0, 90, 0);
            // if (Vector3.Distance(transform.eulerAngles, a) > 0.01f)
            // {
            //     transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, a, Time.deltaTime);
            // }
            // else
            // {
            //     transform.eulerAngles = a;
            // }
            
            
            
            
            if (!blockFlag)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    if (_rotateTimer)
                    {
                        _animator.Play("TurnUp");
                        HorizontalRotate(true);
                        StartCoroutine(ResetRotateTimer());
                    }
                }
                if (Input.GetKey(KeyCode.DownArrow))
                {
                    if (_rotateTimer)
                    {
                        _animator.Play("TurnDown");
                        HorizontalRotate(false);
                        StartCoroutine(ResetRotateTimer());
                    }
                }
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (_rotateTimer)
                    {
                        _animator.Play("TurnRight");
                        VerticalRotate(true);
                        StartCoroutine(ResetRotateTimer());
                    }
                }
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    if (_rotateTimer)
                    {
                        _animator.Play("TurnLeft");
                        VerticalRotate(false);
                        StartCoroutine(ResetRotateTimer());
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
    
    private IEnumerator ResetRotateTimer()
    {

        _rotateTimer = false;
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
                    changedList[i, y1, x1] = Cubes[i, j, k];
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
                    changedList[i, y1, x1] = Cubes[i, j, k];
                }
            }
        }
        Cubes = changedList;
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
