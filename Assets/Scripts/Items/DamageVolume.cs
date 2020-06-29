using UnityEditor.SceneManagement;
using UnityEngine;

public class DamageVolume : MonoBehaviour{

    [Header("Variables")]
    [SerializeField] private float _damageAmount;
    [SerializeField] private float _damageRate;
    [SerializeField] private bool _displayInGame;

    //Variables
    private float _timer;
    private PlayerController _damagee;

    private void Start() {
        //Leaves the Meshrenderers on
        transform.GetComponent<MeshRenderer>().enabled = _displayInGame;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.GetComponent<PlayerController>() != null) {
            _damagee = other.GetComponent<PlayerController>();
        }
    }

    private void OnTriggerExit(Collider other) {
        if(other.GetComponent<PlayerController>() != null) {
            _damagee = null;
        }
    }

    private void FixedUpdate() {
        //Deals damage to whatever player has entered the volume
        if(_damagee != null && _timer < Time.time) {
            _timer = Time.time + _damageRate;
            _damagee.TakeDamage(_damageAmount);
        }
    }

}
