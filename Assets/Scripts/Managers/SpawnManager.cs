using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour{

    [Header("References")]
    [SerializeField] protected GameObject EnemyPrefab;
    [SerializeField] protected List<GameObject> Enemies = new List<GameObject>();

    [Header("Dynamic Variables")]
    [SerializeField] protected bool spawnEnemies = false;
    [SerializeField] protected List<Transform> SpawnPoints = new List<Transform>();
    [SerializeField] protected Vector3 PatrolArea;

    [Header("Zombie Difficulty")]
    [SerializeField] protected float zombieHealth = 50;
    [SerializeField] protected float zombieSpeed = 13;
    [SerializeField] protected float zombieDamage = 15;

    [Header("Variables")]
    [SerializeField] private float _spawnRate = 1;
    [SerializeField] private int _maxZombies = 10;

    //Variables
    private int _zombieCount;
    private bool _gameManagerExists { get => GameManager.Instance != null; }
    private float _timer;

    //Additional Functions
    protected void SpawnZombie(Vector3 position, Vector3 patrolArea){
        //Creates a zombie
        var zombie = Instantiate(EnemyPrefab, position, Quaternion.identity);
        zombie.GetComponent<EnemyFunctions>().PatrolArea = patrolArea;

        //Sets the zombie stats
        zombie.GetComponent<EnemyFunctions>().CurrentHealth = zombieHealth;
        zombie.GetComponent<EnemyFunctions>().ChasingSpeed = zombieSpeed;
        zombie.GetComponent<EnemyFunctions>().AttackingDamage = zombieDamage;

        //Sets the Zombie parent
        if (GameManager.Instance != null)
            zombie.transform.SetParent(GameManager.Instance.ZombieHolder);

        //Adds it to the list
        Enemies.Add(zombie);
    }
    protected void RemoveDeadZombies() {
        _zombieCount = GameManager.Instance.ZombiesKilled;
        Enemies = Enemies.Where(x => x.GetComponent<EnemyFunctions>().IsDead != true).ToList();
    }
    public void UpdateSpawnPoints(List<Transform> newPoints, Vector3 patrolArea) {
        SpawnPoints = newPoints;
        PatrolArea = patrolArea;
    }
    public void RemoveZombie(GameObject zombie) {
        Enemies.Remove(zombie);
    }
    public void SpawnEnemies(bool spawn) {
        spawnEnemies = spawn;
    }
    public void UpdateZombieStats(float health, float speed, float damage) {
        zombieHealth = health;
        zombieSpeed = speed;
        zombieDamage = damage;
    }

    private void FixedUpdate(){
        //Exits if spawning Enemies is false
        if (!spawnEnemies) return;

        //Checks to see if any zombies have been killed and removes them from the list
        if (_gameManagerExists && GameManager.Instance.ZombiesKilled != _zombieCount)
            RemoveDeadZombies();

        //If the player hasn't entered any zones
        if (SpawnPoints.Count == 0) return;

        //Spawns in a new Zombie
        if (_timer < Time.time && Enemies.Count <= _maxZombies){
            _timer = Time.time + _spawnRate;
            var wayPoint = SpawnPoints[Random.Range(0, SpawnPoints.Count)].position;
            SpawnZombie(wayPoint, PatrolArea);
        }
    }

}
