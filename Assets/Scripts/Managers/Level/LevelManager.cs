using UnityEngine;
using System.Collections.Generic;

public class LevelManager : MonoBehaviour{

    [Header("Level Stats")]
    [SerializeField] protected bool _isPlaying;
    [SerializeField] protected int _currentState;
    [SerializeField] protected float _startTime;
    [SerializeField] protected string _nextLevel;

    [Header("Variables")]
    [SerializeField] protected int _counter;
    [SerializeField] protected List<string> _objectiveList = new List<string>();

    [Header("References")]
    [SerializeField] protected GameObject _indicator;

}
