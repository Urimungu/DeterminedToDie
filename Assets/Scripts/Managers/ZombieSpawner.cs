using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour{
    [Header("References")]
    [SerializeField] protected GameObject Zombie;
    [SerializeField] protected List<Transform> SpawnPoints = new List<Transform>();
    [SerializeField] protected List<GameObject> zombies = new List<GameObject>();

    [Header("Variables")]
    [SerializeField] private bool _inRange = false;
    [SerializeField] private float _spawnRate = 1;
    [SerializeField] private int _maxZombies = 10;

    //Variables
    private float _timer;
    private int _prevZombies;

    private void FixedUpdate(){
        if (_timer < Time.time && zombies.Count <= _maxZombies && _inRange) {
            _timer = Time.time + _spawnRate;
            SpawnZombie();
        }

        if (GameManager.Instance != null && GameManager.Instance.ZombiesKilled != _prevZombies) {
            _prevZombies = GameManager.Instance.ZombiesKilled;
            zombies = zombies.Where(x => x.GetComponent<EnemyFunctions>().IsDead != true).ToList();
        }

    }

    public void SpawnZombie() {
        //Creates a zombie and adds it to the list
        var wayPoint = Random.Range(0, SpawnPoints.Count);
        var zombie = Instantiate(Zombie, SpawnPoints[wayPoint].position, Quaternion.identity);
        zombies.Add(zombie);
    }

    private void OnTriggerEnter(Collider other){
        if (other.GetComponent<CharacterFunctions>() != null)
            _inRange = true;
    }

    private void OnTriggerExit(Collider other){
        if (other.GetComponent<CharacterFunctions>() != null)
            _inRange = false;
        
    }
}
