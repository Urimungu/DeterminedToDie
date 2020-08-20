using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyFunctions : EnemyStats {

    //Health Stats
    public float CurrentHealth {
        get => _currentHealth;
        set {
            _currentHealth = value;

            //Kills the enemy
            if (_currentHealth <= 0)
                CurrentState = State.Death;
        }
    }
    public float MaxHealth {
        get => _maxHealth;
        set => _maxHealth = value;
    }
    public float StaggerChance {
        get => _staggerChance;
        set => _staggerChance = value;
    }
    public float PatrollingSpeed {
        get => _patrollingSpeed;
        set => _patrollingSpeed = value;
    }
    public float ChasingSpeed {
        get => _chasingSpeed;
        set => _chasingSpeed = value;
    }

    //Dynamic Variables
    public float CurrentSpeed {
        get => _currentSpeed;
        set => _currentSpeed = value;
    }

    //Attacking
    public float AttackingDamage {
        get => _attackingDamage;
        set => _attackingDamage = value;
    }
    public float AttackRate {
        get => _attackRate;
        set => _attackRate = value;
    }
    public float AttackDistance {
        get => _attackDistance;
        set => _attackDistance = value;
    }
    public float AlertTime {
        get => _alertTime;
        set => _alertTime = value;
    }

    //Variables
    public bool CanMove {
        get => _canMove;
        set => _canMove = value;
    }
    public float CheckGroundRay {
        get => _checkGroundRay;
        set => _checkGroundRay = value;
    }
    public float DetectDistance {
        get => _detectDistance;
        set => _detectDistance = value;
    }
    public float DetectFallOfDistance {
        get => _detectFallOfDistance;
        set => _detectFallOfDistance = value;
    }
    public LayerMask GroundMask {
        get => _groundMask;
    }

    //Patrolling
    public float PatrolRadius {
        get => _patrolRadius;
        set => _patrolRadius = value;
    }
    public Vector3 PatrolArea {
        get => _patrolArea;
        set => _patrolArea = value;
    }
    public Vector3 PatrolPoint{
        get{
            //Selects a new point
            if (_patrolPoint == Vector3.zero){
                var randomPos = Random.insideUnitSphere * PatrolRadius;
                randomPos += PatrolArea;

                //Navmesh Detection
                NavMesh.SamplePosition(randomPos, out NavMeshHit hit, PatrolRadius, 1);
                _patrolPoint = hit.position;
                _patrolPoint.y = transform.position.y;
            }

            return _patrolPoint;
        }
        set => _patrolPoint = value;
    }

    //Aesthetics
    public Material PrevMat {
        get => _prevMat != null ? _prevMat : _prevMat = transform.Find("Mesh/Model").GetComponent<SkinnedMeshRenderer>().material;
        set => _prevMat = transform.Find("Mesh/Model").GetComponent<SkinnedMeshRenderer>().material;
    }
    public Material DamagedMaterial {
        get => _damagedMaterial;
        set => _damagedMaterial = value;
    }
    public float BodyDecay {
        get => _bodyDecay;
    }

    //References
    public Transform Target {
        get => _target;
        set => _target = value;
    }
    public NavMeshAgent Agent {
        get => _agent != null ? _agent : _agent = GetComponent<NavMeshAgent>();
    }
    public Animator Anim {
        get => _anim != null ? _anim : _anim = transform.Find("Mesh").GetComponent<Animator>();
    }
    public SpawnManager Spawner {
        get => _spawner;
        set => _spawner = value;
    }
    
    //State Machine
    public State CurrentState {
        get => _currentState;
        set {
            switch (value) {
                case State.Nothing:
                    break;
                case State.Patrol:
                    break;
                case State.Chase:
                    break;
                case State.Death:
                    Death();
                    break;
            }

            _currentState = value;
        }
    }

    //Additional Varaibles
    protected float _alertTimer;
    protected bool _arrivedAtDestination = false;
    public bool IsDead = false;

    //Additional Functions
    protected bool DetectPlayer(){
        //Gets a list of all the colliders around the enemy
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, DetectDistance);

        //Checks to see which player is closer
        foreach (Collider collider in hitColliders){
            if (collider.GetComponent<PlayerController>() != null){
                Target = collider.transform;
                CurrentSpeed = ChasingSpeed;
                return true;
            }
        }

        CurrentSpeed = PatrollingSpeed;
        return false;
    }
    protected void Attack(){
        //Calculates the Attack Logic
        var center = transform.position + (transform.forward * AttackDistance);
        var size = new Vector3(1.5f, 2, 1.5f);

        //Checks everything it might hit in the process
        Collider[] damageTakers = Physics.OverlapBox(center, size);

        //Applies the damage to the players in the attack radius
        foreach (Collider col in damageTakers){
            if (col.transform.GetComponent<PlayerController>() != null)
                col.transform.GetComponent<PlayerController>().TakeDamage(AttackingDamage);
        }
    }

    public void TakeDamage(float damage, Transform shooter){
        //Alerts the Enemy
        if (shooter != null){
            _alertTimer = Time.time + AlertTime;
            Target = shooter;
            CurrentState = State.Chase;
        }

        //Calculates Damage
        CurrentHealth -= damage;

        //Shows Damage Aesthetics
        StartCoroutine(TakeDamageAesthetics());
    }
    public void Death() {
        //Makes sure it doesn't create too many Ragdolls
        if (IsDead) return;
        //Sets values
        IsDead = true;
        CanMove = false;

        //Replaces the Current Model with the Ragdoll
        Instantiate(Resources.Load<GameObject>("Prefabs/Ragdoll/ZombieRagdoll"), transform.position, transform.rotation, transform);
        transform.Find("Mesh").gameObject.SetActive(false);

        //Blocks Movement
        Agent.enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        StartCoroutine(KillEnemy());

        //Adds itself to the kill counter
        if (GameManager.Instance != null)
            GameManager.Instance.ZombiesKilled++;
    }

    IEnumerator KillEnemy()
    {
        yield return new WaitForSeconds(BodyDecay);
        Destroy(gameObject);
    }
    IEnumerator TakeDamageAesthetics(){
        transform.Find("Mesh/Model").GetComponent<SkinnedMeshRenderer>().material = DamagedMaterial;
        yield return new WaitForSeconds(0.05f);
        transform.Find("Mesh/Model").GetComponent<SkinnedMeshRenderer>().material = PrevMat;
    }
}
