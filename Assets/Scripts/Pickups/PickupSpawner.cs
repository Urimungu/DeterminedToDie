using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Random Spawn List")]
    public List<GameObject> pickupPrefs = new List<GameObject>();

    [Header("Static Spawn Pickup")]
    public GameObject pickupPre;

    [Header("Delay")]
    public float spawnDel;

    [Header("Set Spawn Type")]
    public bool randomSpawn;

    private GameObject curPickup;
    private float nextSpawn;
    private Transform tf;

    void Start()
    {
        tf = GetComponent<Transform>();
        SpawnPickups();
        nextSpawn = Time.time + spawnDel;
    }

    void Update()
    {
        if (Time.time >= nextSpawn && curPickup == null)
            SpawnPickups();
        else if (curPickup != null)
            nextSpawn = Time.time + spawnDel;
    }

    void SpawnPickups()
    {
        if (randomSpawn)
            pickupPre = pickupPrefs[Random.Range(0, pickupPrefs.Count)];
        else

            curPickup = Instantiate(pickupPre, tf.position, Quaternion.identity);
        nextSpawn = Time.time + spawnDel;
    }
}
