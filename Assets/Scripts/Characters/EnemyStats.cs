using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour{

    [Header("Stats")]
    public float WalkingSpeed = 10;
    public float RunningSpeed = 15;

    [Header("Variables")]
    public bool CanMove = true;
    public float CheckGroundRay = 0.3f;
    public LayerMask GroundMask;
}
