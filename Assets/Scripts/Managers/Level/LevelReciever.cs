using UnityEngine;

public class LevelReciever : LevelFunctions{

    //Temporary Variables
    private float _waitStartTime;
    private bool _completedStartFunctions;

    //Killing Variables
    private int _prevKills;
    private int _neededKills;
    private int _startingZombieCount;

    //Begins the Game
    private void Start(){
        //Starts the Game
        CurrentState = 0;
        StartGame(StartTime);
        UpdateUI(ObjectiveDetails[CurrentState].Objective);
    }

    //Runs the StateMachine
    private void FixedUpdate(){
        if (IsPlaying) ObjectiveStateMachine(CurrentState);
    }
    private void ObjectiveStateMachine(int ID) {
        //Decides what it's looking for
        switch (Objectives[ID]) {
            case ObjectiveStruct.ObjectiveType.Destination:     Destination(ID);    break;
            case ObjectiveStruct.ObjectiveType.Wait:            Wait(ID);           break;
            case ObjectiveStruct.ObjectiveType.PickUp:          PickUp(ID);         break;
            case ObjectiveStruct.ObjectiveType.Kill:            KillEnemies(ID);    break;
            case ObjectiveStruct.ObjectiveType.ExitGame:        ExitGame(ID);       break;

            case ObjectiveStruct.ObjectiveType.Nothing:
            default: break;
        }
    }

    //Possible Functions
    private void KillEnemies(int ID) {
        //Start Condition
        if (!_completedStartFunctions) {
            StartFunctions(ID);
            CanZombiesSpawn(true);

            print("Ran");

            _startingZombieCount = GameManager.Instance.ZombiesKilled;
            _prevKills = GameManager.Instance.ZombiesKilled;
            _neededKills = ObjectiveDetails[ID].EnemiesToKill;
            var tempText = ObjectiveDetails[ID].Objective.Replace("#", "<Color=#ffd829>" + _neededKills.ToString("0") + "</color>");
            UpdateUI(tempText);
        }

        //Prevents it from running everything every single frame
        if (GameManager.Instance.ZombiesKilled == _prevKills) return;

        //Sets Values
        _prevKills = GameManager.Instance.ZombiesKilled;
        var currentKilled = (GameManager.Instance.ZombiesKilled - _startingZombieCount);

        //Exit Condition
        if (currentKilled >= _neededKills){
            Proceed();

            //Runs the exit functions
            ExitFunctions(ID);

            return;
        }

        //Updates Display
        var message = ObjectiveDetails[CurrentState].Objective.Replace("#", "<Color=#ffd829>" + (ObjectiveDetails[ID].EnemiesToKill - currentKilled).ToString("0") + "</color>");
        UpdateSideObjective(message);
    }
    private void Destination(int ID) {
        //StartConditions
        if (!_completedStartFunctions) StartFunctions(ID);

        //Exit Condition
        if (IsInRange(ObjectiveDetails[ID].Destination, ObjectiveDetails[ID].DestinationDistance)){
            //Continues
            Proceed();

            //Exit Functions
            ExitFunctions(ID);

            return;
        }

        //Shows the Location of the Destination
        SetIndicatorPosition(ObjectiveDetails[ID].Destination.position);
    }
    private void Wait(int ID) {
        //Initializes if it just started
        if (!_completedStartFunctions) { 
            _waitStartTime = Time.time + ObjectiveDetails[ID].WaitTime;
            StartFunctions(ID);
        }

        //Exit Condition
        if (Time.time > _waitStartTime) {
            Proceed();
            _waitStartTime = -1;
        }
    }
    private void PickUp(int ID) {
        //Start Conditions
        if (!_completedStartFunctions) StartFunctions(ID);

        //Exit Condition
        if (ObjectiveDetails[ID].PickUp == null) {
            Proceed();

            //Completes the Exit Functions
            ExitFunctions(ID);

            return;
        }

        //Sets the position of the indictor
        SetIndicatorPosition(ObjectiveDetails[ID].PickUp.position);
    }
    private void ExitGame(int ID) {
        //Exit Condition
        if (!_isEnding) EndLevel(ObjectiveDetails[ID].LeveltoLoad, ObjectiveDetails[ID].ExitTime);
        
    }

    //Checks for the Exit and Startfunctions
    private void ExitFunctions(int ID) {
        //On Exit Remove Object
        if (ObjectiveDetails[ID].RemoveObject == ObjectiveStruct.ObjectActionType.Exit)
            ObjectiveDetails[ID].Remove.SetActive(false);

        //On Exit Return Object
        if (ObjectiveDetails[ID].ReturnObject == ObjectiveStruct.ObjectActionType.Exit)
            ObjectiveDetails[ID].Return.SetActive(true);

        _completedStartFunctions = false;
    }
    private void StartFunctions(int ID) {
        //Remove on Start
        if (ObjectiveDetails[ID].RemoveObject == ObjectiveStruct.ObjectActionType.Start)
            ObjectiveDetails[ID].Remove.SetActive(false);

        //Return On Start
        if (ObjectiveDetails[ID].ReturnObject == ObjectiveStruct.ObjectActionType.Start)
            ObjectiveDetails[ID].Return.SetActive(true);

        _completedStartFunctions = true;
    }
}
