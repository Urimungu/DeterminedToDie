using UnityEngine;
using UnityEngine.AI;

public class EnemyStats : MonoBehaviour {

    [Header("Health Stats")]
    [SerializeField] protected float _currentHealth = 100;
    [SerializeField] protected float _maxHealth = 100;
    [SerializeField] protected float _staggerChance = 0.25f;
    [SerializeField] protected float _patrollingSpeed = 7;
    [SerializeField] protected float _chasingSpeed = 20;

    [Header("Dynamic Variables")]
    [SerializeField] protected float _currentSpeed = 7;

    [Header("Attacking")]
    [SerializeField] protected float _attackingDamage = 30;
    [SerializeField] protected float _attackRate = 0.5f;
    [SerializeField] protected float _attackDistance = 1;
    [SerializeField] protected float _alertTime = 5;

    [Header("Variables")]
    [SerializeField] protected bool _canMove = true;
    [SerializeField] protected float _checkGroundRay = 0.3f;
    [SerializeField] protected float _detectDistance = 10;
    [SerializeField] protected float _detectFallOfDistance = 15;
    [SerializeField] protected LayerMask _groundMask;

    [Header("Patrolling")]
    [SerializeField] protected float _patrolRadius = 20;
    [SerializeField] protected Vector3 _patrolPoint;
    [SerializeField] protected Transform _patrolArea;

    [Header("Aesthetics")]
    [SerializeField] protected Material _prevMat;
    [SerializeField] protected Material _damagedMaterial;
    [SerializeField] protected float _bodyDecay = 25;

    [Header("References")]
    [SerializeField] protected Transform _target;
    [SerializeField] protected NavMeshAgent _agent;
    [SerializeField] protected Animator _anim;

    [Header("StateMachine")]
    [SerializeField] protected State _currentState;

    public enum State { Nothing, Patrol, Chase, Attack, Death, EatPlayer }
}
