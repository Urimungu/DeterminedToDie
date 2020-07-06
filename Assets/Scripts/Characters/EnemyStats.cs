using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour {

    [Header("Health Stats")]
    public float CurrentHealth = 100;
    public float MaxHealth = 100;

    [Header("Stats")]
    public float WalkingSpeed = 10;
    public float RunningSpeed = 15;

    [Header("Attacking")]
    public float AttackingDamage = 30;
    public float AttackRate = 0.5f;
    public float AttackDistance = 1;
    public float AlertTime = 5;

    [Header("Variables")]
    public bool CanMove = true;
    public float CheckGroundRay = 0.3f;
    public float DetectDistance = 10;
    public float DetectFallOfDistance = 15;
    public LayerMask GroundMask;

    [Header("References")]
    public Material DamageMaterial;
    private Rigidbody _enemyRigidBody;

    #region Properties
    public Rigidbody EnemyRigidBody{
        get{
            if(_enemyRigidBody == null)
                _enemyRigidBody = GetComponent<Rigidbody>();
            return _enemyRigidBody;
        }
    }
    #endregion
}
