using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour{

    [Header("Level Stats")]
    [SerializeField] protected bool _isPlaying;
    [SerializeField] protected float _startTime;
    [SerializeField] protected int _currentState;

    [Header("Variables")]
    [SerializeField] protected List<ObjectiveStruct.ObjectiveType> _objectiveList = new List<ObjectiveStruct.ObjectiveType>();
    [SerializeField] protected List<ObjectiveStruct> _objectiveDetails = new List<ObjectiveStruct>();
    [SerializeField] protected List<bool> _selectedItems = new List<bool>();
 
    [Header("References")]
    [SerializeField] protected GameObject _indicator;
    [SerializeField] protected SpawnManager _spawner;

}
