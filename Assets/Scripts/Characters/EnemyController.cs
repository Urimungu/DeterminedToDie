using System.Collections;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
        switch(CurrentState) {
            case State.Patrol:      Patrol();       break;
            case State.Chase:       Chase();        break;
            case State.EatPlayer:   EatPlayer();    break;

            case State.Nothing:
            default:    break;
        }
    }

    //Patrols in an area
    private void Patrol() {
        //Patrols
        if (DetectPlayer()) 
            CurrentState = State.Chase;

        //Exits if the player is already at the destination
        if (_arrivedAtDestination || PatrolArea == null) return;

        //Walks towards the patrol point
        Agent.SetDestination(PatrolPoint);

        //Got to the destination and is stopping for a couple seconds
        var dist = (transform.position - PatrolPoint).magnitude;
        if (dist <= 2) {
            _arrivedAtDestination = true;
            StartCoroutine(GetToDestination(Random.Range(1f, 4f))); 
        }
    }

    //resets the point after a certain amount of time
    private IEnumerator GetToDestination(float timer) {
        yield return new WaitForSeconds(timer);
        PatrolPoint = Vector3.zero;
        _arrivedAtDestination = false;
    }

    //Chases whatever it has found
    private void Chase() {
        //Makes sure that the target isn't equal to null
        if (Target == null) {
            CurrentState = State.Patrol;
            return;
        }

        //The player is dead
        if (Target.GetComponent<CharacterFunctions>().IsDead) {
            if (Random.Range(0f, 1f) > 0.8f){
                Agent.speed = 7;
                Agent.acceleration = 8;
                Agent.stoppingDistance = 2;
                CurrentState = State.EatPlayer;
                Target = Target.GetComponent<CharacterFunctions>().Corpse.transform.Find("Body");
            }
            else
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
            Anim.SetFloat("AttackSpeed", Random.Range(0.3f, 1f));
            Anim.SetTrigger("Attack");
            _attacked = false;
        }else{
            var newPos = Target.position;
            Agent.speed = CurrentSpeed;
            Agent.SetDestination(newPos);
        }
    }

    //Eats the player
    private void EatPlayer() {
        //Attacks the enemy
        var dist = (Target.position - transform.position).magnitude;
        if (dist < 2){
            print("Eating");
            Anim.SetTrigger("Eat");
        }else{
            Agent.speed = CurrentSpeed;
            Agent.SetDestination(Target.position);
        }
    }

    //Updates the Enemy Animations
    private void UpdateAnimations() {
        Anim.SetFloat("Speed", (new Vector3(Agent.velocity.x, 0, Agent.velocity.z)).magnitude);
    }
}
