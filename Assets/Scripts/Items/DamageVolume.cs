using UnityEditor.SceneManagement;
using UnityEngine;

public class DamageVolume : MonoBehaviour{

    [Header("Variables")]
    [SerializeField] private float _damageAmount = 0.1f;
    [SerializeField] private float _damageRate = 0.4f;
    [SerializeField] private bool _gainHealth = false;
    [SerializeField] private bool _displayInGame = false;

    //Variables
    private float _timer;
    private PlayerController _damagee;

    private void Start() {
        //Leaves the Meshrenderers on
        transform.GetComponent<MeshRenderer>().enabled = _displayInGame;
    }

    //The player is within the volume and should be taking damage
    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PlayerController>() != null) {
            _damagee = other.GetComponent<PlayerController>();
        }
    }

    //The player is no longer inside the volume
    private void OnTriggerExit(Collider other) {
        if(other.GetComponent<PlayerController>() != null) {
            _damagee = null;
        }
    }

    private void FixedUpdate() {
        //Deals damage to whatever player has entered the volume
        if(_damagee != null && _timer < Time.time) {
            _timer = Time.time + _damageRate;
            _damagee.TakeDamage(_gainHealth ? _damageAmount * -1 : _damageAmount);
        }
    }

}
