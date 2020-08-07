using UnityEngine;

public class LevelOne : LevelFunctions{

    [Header("Specific Stats")]
    [SerializeField] protected GameObject Weapon;
    [SerializeField] protected Transform WayPointOne;
    [SerializeField] protected Transform WayPointTwo;
    [SerializeField] protected int neededKills;

    //Varibles
    private int _startingZombieCount;
    private int _prevKilled = -1;
    

    private void Start(){
        CurrentState = 0;

        //Starts the Game
        StartCoroutine(StartGame(StartTime));

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
            Proceed();
            return;
        }

        //Tells the Player to pick up the weapon
        SetIndicatorPosition(Weapon.transform.position);
    }
    private void EnterFightingZone() {
        var distance = (GameManager.Instance.Player.transform.position - WayPointOne.position).magnitude;
        if (distance < 5) {
            ShowIndicator(false);
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
            var tempText = Objectives[CurrentState].Replace("#", "<Color=#ffd829>" + neededKills.ToString("0") + "</color>");
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
        var message = Objectives[CurrentState].Replace("#", "<Color=#ffd829>" + (neededKills - currentKilled).ToString("0") + "</color>");
        UpdateSideObjective(message);
    }
    private void EnterSafeHouse() {
        var distance = (GameManager.Instance.Player.transform.position - WayPointTwo.position).magnitude;
        if (distance < 5){
            ShowIndicator(false);
            Proceed();
            return;
        }

        //Tells the Player to pick up the weapon
        SetIndicatorPosition(WayPointTwo.position);
    }
    private void LeaveGame(){
        //Exits the game
        if (!_isEnding)
            EndLevel(NextLevel, 3);
    }
}
