using UnityEngine;

public class EnemyController : EnemyFunctions {

    //Variables
    private float _attackTimer;
    private bool _attacked;

    private void Start() {
        PrevMat = null;
    }

    private void Update() {
        if (CanMove){
            StateMachine();
            UpdateAnimations();
        }
    }

    //Controls the Enemy
    private void StateMachine() {
        switch(_currentState) {
            case State.Patrol:  
                Patrol();   
                break;
            case State.Chase:     
                Chase();    
                break;

            //If it's set to nothing or it doesn't fit the requirements it does nothing
            case State.Nothing:
            default:
                break;
        }
    }

    //Patrols in an area
    private void Patrol() {
        //Patrols
        if (DetectPlayer()) 
            CurrentState = State.Chase;
        
    }

    //Chases whatever it has found
    private void Chase() {
        //Makes sure that the target isn't equal to null
        if (Target == null) {
            CurrentState = State.Patrol;
            return;
        }

        //Returns if the enemy already attacked
        if (Time.time <= _attackTimer) {
            var animation = Anim.GetCurrentAnimatorStateInfo(0);
            if (animation.IsName("Attack") && animation.normalizedTime > 0.35f && animation.normalizedTime < 0.5f && !_attacked) {
                _attacked = true;
                Attack();
            }
            return;
        }

        //Stops chasing the enemy
        var dist = (Target.position - transform.position).magnitude;
        if (dist > DetectFallOfDistance && Time.time > _alertTimer){
            Target = null;
            return;
        }else
            _alertTimer = Time.time + AlertTime;

        //Attacks the enemy
        if (dist < AttackDistance){
            _attackTimer = Time.time + AttackRate;
            Anim.SetTrigger("Attack");
            _attacked = false;
        }else{
            var newPos = Target.position;
            Agent.SetDestination(newPos);
        }
    }

    //Updates the Enemy Animations
    private void UpdateAnimations() {
        Anim.SetFloat("Speed", (new Vector3(Agent.velocity.x, 0, Agent.velocity.z)).magnitude);
    }
}
