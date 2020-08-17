using System.Collections.Generic;
using UnityEngine;

public class LevelOne : LevelFunctions{

    [Header("Specific Stats")]
    [SerializeField] protected GameObject Weapon;
    [SerializeField] protected GameObject FirstDoor;
    [SerializeField] protected Transform WayPointOne;
    [SerializeField] protected Transform WayPointTwo;
    [SerializeField] protected int neededKills;

    //Varibles
    private int _startingZombieCount;
    private int _prevKilled = -1;


    private void Start(){
        CurrentState = 0;

        //Starts the Game
        StartGame(StartTime);

        //Sets itself in the game manager
        if (GameManager.Instance != null)
            GameManager.Instance.CurrentLevel = this;
    }

    private void FixedUpdate(){
        if(IsPlaying)
            StateMachine();
    }

    //Level Manager State Machine
    private void StateMachine() {
        switch (CurrentState) {
            case 1: PickUpWeapon();         break;
            case 2: EnterFightingZone();    break;
            case 3: InBetween();            break;
            case 4: KillZombies();          break;
            case 5: EnterSafeHouse();       break;
            case 6: LeaveGame();            break;
            default:    /*Does nothing*/    break;
        }
    }

    private void PickUpWeapon() {
        if (Weapon == null) {
            //Hides the Indicator
            ShowIndicator(false);

            //Updates zombies
            FirstDoor.SetActive(false);
            UpdateZombies(50, 7, 15);
            CanZombiesSpawn(true);

            //Proceed to the next objective
            Proceed();
            return;
        }

        //Tells the Player to pick up the weapon
        SetIndicatorPosition(Weapon.transform.position);
    }
    private void EnterFightingZone() {
        //Exit Parameter
        if (IsInRange(WayPointOne, 5)) {
            //Turns off the waypoint indicator
            ShowIndicator(false);

            //Closes the previous door
            FirstDoor.SetActive(true);

            //Proceeds to the next level
            Proceed();
            return;
        }

        //Tells the Player to pick up the weapon
        SetIndicatorPosition(WayPointOne.position);
    }
    private void InBetween() {
        if (!_isProceeding)
            StartCoroutine(ProceedTimer(1));
        
    }
    private void KillZombies() {
        //Prevents it from running everything every single frame
        if (GameManager.Instance.ZombiesKilled == _prevKilled) return;

        //First Run Through
        if (_prevKilled < 0) {
            _startingZombieCount = GameManager.Instance.ZombiesKilled;
            _prevKilled = GameManager.Instance.ZombiesKilled;
            var tempText = ObjectiveDetails[CurrentState].Objective.Replace("#", "<Color=#ffd829>" + neededKills.ToString("0") + "</color>");
            UpdateUI(tempText);
            return;
        }

        //Sets Values
        _prevKilled = GameManager.Instance.ZombiesKilled;
        var currentKilled = (GameManager.Instance.ZombiesKilled - _startingZombieCount);

        //Exit Condition
        if (currentKilled >= neededKills) {
            Proceed();
            return;
        }

        //Updates Display
        var message = ObjectiveDetails[CurrentState].Objective.Replace("#", "<Color=#ffd829>" + (neededKills - currentKilled).ToString("0") + "</color>");
        UpdateSideObjective(message);
    }
    private void EnterSafeHouse() {
        //Exit Condition
        if (IsInRange(WayPointTwo, 5)){
            //Turns off the display
            ShowIndicator(false);

            //Proceeds to the next Objective
            Proceed();
            return;
        }

        //Tells the Player to pick up the weapon
        SetIndicatorPosition(WayPointTwo.position);
    }
    private void LeaveGame(string nextLevel = ""){
        //Exits the game
        if (!_isEnding)
            EndLevel(nextLevel, 3);
    }
}
