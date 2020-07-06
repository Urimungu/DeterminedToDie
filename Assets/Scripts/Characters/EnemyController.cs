using System.Collections;
using UnityEngine;

public class EnemyController : EnemyStats {

    //Variables
    private bool _isRunning;
    private float _attackTimer;
    private float _alertTimer;
    private Material _prevMat;
    public Material PrevMat {
        get {
            if(_prevMat == null)
                _prevMat = transform.GetChild(0).GetComponent<MeshRenderer>().material;
            return _prevMat;
        }
        set {
            _prevMat = transform.GetChild(0).GetComponent<MeshRenderer>().material;
        }

    }
    public enum State { Patrolling, Chasing }

    [Header("Variables")]
    [SerializeField] private State _state;
    [SerializeField] private Transform _target;
    public Transform Target {
        get {
            //Sets the enemy for patrolling if there is no enemy
            if(_target == null) 
                _state = State.Patrolling;
            return _target;
        }
        set {
            //Sets the Enemy to follow the state
            if(value != null) 
                _state = State.Chasing;
            
            _target = value;
        }
    }
    private void Start() {
        PrevMat = null;
    }

    private void FixedUpdate() {
        if(CanMove) 
            StateMachine();
        
    }

    //Controls the Enemy
    private void StateMachine() {
        switch(_state) {
            case State.Patrolling:  Patrol();   break;
            case State.Chasing:     Chase();    break;
        }
    }

    //Patrols in an area
    private void Patrol() {
        //Patrols
        DetectPlayer();
    }

    //Detects if the player is inrange of the enemy
    private bool DetectPlayer() {
        //Gets a list of all the colliders around the enemy
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, DetectDistance);

        //Checks to see which player is closer
        foreach(Collider collider in hitColliders) {
            if(collider.GetComponent<PlayerController>() != null)
                Target = collider.transform;   
        }

        return false;
    }

    //Chases whatever it has found
    private void Chase() {
        //Makes sure that the target isn't equal to null
        if(Target == null) {
            _state = State.Patrolling;
            return;
        }

        //Returns if the enemy already attacked
        if(Time.time <= _attackTimer) return;

        //Moves the enemy towards the player
        var newPos = Target.position;
        Movement.Move(newPos, this);

        //Stops chasing the enemy
        var dist = (Target.position - transform.position).magnitude;
        if(dist < DetectFallOfDistance && Time.time > _alertTimer)
            Target = null;

        //Attacks the enemy
        if(dist < AttackDistance) {
            _attackTimer = Time.time + AttackRate;
            Attack();
        }
    }

    //Attacks whomever it's targeted on
    private void Attack() {
        //Calculates the Attack Logic
        var center = transform.position + (transform.forward * AttackDistance);
        var size = new Vector3(1, 1, 1);

        Collider[] damageTakers = Physics.OverlapBox(center, size);

        //Applies the damage to the players in the attack radius
        foreach(Collider col in damageTakers) {
            if(col.transform.GetComponent<PlayerController>() != null)
                col.transform.GetComponent<PlayerController>().TakeDamage(AttackingDamage);
        }
    }

    public void TakeDamage(float damage, Transform shooter = null) {
        //Alerts the Enemy
        if(shooter != null) {
            _alertTimer = Time.time + AlertTime;
            Target = shooter; 
        }

        //If the player can't move then it can't take damage
        if(!CanMove) return;

        CurrentHealth -= damage;

        //Shows Damage Marker
        if(!_isRunning) {
            _isRunning = true;
            StartCoroutine(TakeDamage());
        }

        //Player Dies
        if(CurrentHealth <= 0) {
            Death();
        }
    }

    //The enemy dies
    private void Death() {
        CanMove = false;
        EnemyRigidBody.constraints = RigidbodyConstraints.None;
        StartCoroutine(KillEnemy());

        //Initializes the Enemy falling backwards
        var direction = -Vector3.forward * 2;

        //Knocks the Enemy Over away from the player if there is one
        if(GameManager.Instance != null && GameManager.Instance.Player != null) 
            direction = (GameManager.Instance.Player.transform.position - transform.position).normalized;
        
        //Applies the Force
        EnemyRigidBody.AddForce(-direction * 5, ForceMode.Impulse);
    }

    IEnumerator KillEnemy() {
        yield return new WaitForSeconds(5);
        Destroy(gameObject);
    }

    //Aesthetics of taking damage
    IEnumerator TakeDamage() {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = DamageMaterial;
        yield return new WaitForSeconds(0.1f);
        transform.GetChild(0).GetComponent<MeshRenderer>().material = PrevMat;
        _isRunning = false;
    }
}
