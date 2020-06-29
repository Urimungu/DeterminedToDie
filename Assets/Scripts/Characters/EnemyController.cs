using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : EnemyStats {

    //Variables
    private bool _isRunning;
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

    private void Start() {
        PrevMat = null;
    }

    private void Update() {
        if(CanMove) {
            //Movement
        }
    }

    public void TakeDamage(float damage) {
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
