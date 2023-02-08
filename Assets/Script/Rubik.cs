using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rubik : MonoBehaviour
{
    public GameObject[,,] Cubes=new GameObject[3,3,3];
    public bool moveAvailability;
    public bool rotateAbility=true;
    public Vector3 rotateAngle;
    public bool stopMoving;
    public bool blockFlag = false;
    public float moveTimeGap;
    public float moveSpeed;
    public float rotateSpeed;
    protected bool _moveForwardFlag=true;
    protected bool _moveVerticalFlag=false;
    protected bool _moveVerticalTimer = true;
    protected bool _rotateLeft=false;
    protected bool _rotateRight=false;
    protected bool _rotateUp=false;
    protected bool _rotateDown = false;
    protected Quaternion _currentQuaternion;
    protected Quaternion _targetQuaternion;
    [SerializeField] protected Block currentBlock;
    [SerializeField] protected Block targetBlock;
    [SerializeField] protected Block verticalTargetBlock;
    public virtual void SetActiveCube(Vector3 input){}
    
    public virtual void VerticalRotateTopTier(bool dir){}
}
