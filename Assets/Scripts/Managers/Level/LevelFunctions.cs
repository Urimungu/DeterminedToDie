using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelFunctions : LevelManager {

    //Stats
    public bool IsPlaying {
        get => _isPlaying;
        set => _isPlaying = value;
    }
    public int CurrentState {
        get => _currentState;
        set => _currentState = value;
    }
    public float StartTime {
        get => _startTime;
        set => _startTime = value;
    }
    public string NextLevel {
        get => _nextLevel;
    }

    //Variables
    public int Counter {
        get => _counter;
        set => _counter = value;
    }
    public List<string> Objectives {
        get => _objectiveList;
        set => _objectiveList = value;
    }

    //References
    public GameObject Indicator {
        get {
            if (_indicator == null) {
                var indicator = Instantiate(Resources.Load<GameObject>("UI/Arrow")).transform;
                indicator.SetParent(GameObject.Find("Canvas").transform);
                _indicator = indicator.gameObject;
            }

            return _indicator;
        }
    }

    //Protected variables
    protected bool _isProceeding;
    protected bool _isEnding;

    //Functions
    public void EndLevel(string levelName, float exitTimer) {
        StartCoroutine(LevelEnder(levelName, exitTimer));
    }
    public void Proceed() {
        //Updates the values here
        CurrentState++;
        Counter = 0;
        UpdateUI(Objectives[CurrentState]);
    }

    //Protected Functions
    protected void ShowIndicator(bool show = true) {
        Indicator.SetActive(show);
    }
    protected void SetIndicatorPosition(Vector3 position) {
        //Checks if the object to view is infront or behind
        var dir = position - Camera.main.transform.position;
        var dot = Vector3.Dot(dir, Camera.main.transform.forward);
        if (dot < 0) return; 

        //Turns itself on if it's off
        if (!Indicator.gameObject.activeInHierarchy)
            ShowIndicator();

        //Moves itself ontop of the required position
        var newPos = Camera.main.WorldToScreenPoint(position);
        Indicator.GetComponent<RectTransform>().anchoredPosition = newPos;
    }
    protected void FreePlayer(bool free = true) {
        if (GameManager.Instance != null && GameManager.Instance.Player != null)
            GameManager.Instance.Player.CanRecieveInput = free;

    }
    protected void UpdateUI(string message) {
        if (GameManager.Instance != null && GameManager.Instance.HUD != null) 
            GameManager.Instance.HUD.UpdateObjectiveDisplay(message);
        
    }
    protected void UpdateSideObjective(string message) {
        if (GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdateSideObjective(message);
        
    }

    //Waiting Functions
    protected IEnumerator LevelEnder(string levelName, float exitTime){
        _isEnding = true;
        yield return new WaitForSeconds(exitTime);
        SceneManager.LoadScene(levelName);
        FreePlayer(false);
        _isEnding = false;
    }
    protected IEnumerator StartGame(float timer) {
        yield return new WaitForSeconds(timer);
        FreePlayer();
        Proceed();
    }
    protected IEnumerator ProceedTimer(float timer) {
        _isProceeding = true;
        yield return new WaitForSeconds(timer);
        Proceed();
        _isProceeding = false;
    }
}
