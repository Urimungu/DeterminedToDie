using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : MonoBehaviour {

    [Header("References")]
    [SerializeField] protected Transform patrolArea;
    [SerializeField] protected List<Transform> SpawnPoints = new List<Transform>();
    [SerializeField] protected SpawnManager Manager;

    //Variables
    private bool _didUpdate;

    //When the player enters this Trigger, this becomes the new spawning location for the zombies
    private void OnTriggerStay(Collider other){
        if (!_didUpdate && other.GetComponent<CharacterFunctions>() != null){
            _didUpdate = true;
            Manager.UpdateSpawnPoints(SpawnPoints, patrolArea.position);
        }
    }

    private void OnTriggerExit(Collider other){
        if (_didUpdate && other.GetComponent<CharacterFunctions>() != null)
            _didUpdate = false;
    }
}
