using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class HUDFunctions : HUDManager{

    //Cross Hair
    public Image CrossHair {
        get => _crossHair != null ? _crossHair : _crossHair = transform.Find("MainDisplay/CrossHair").GetComponent<Image>();
    }

    //Health
    public GameObject HealthHolder { 
        get => _healthHolder != null ? _healthHolder : _healthHolder = transform.Find("MainDisplay/Health").gameObject;
    }
    public Color HealthFillColor {
        get => _healthFillColor != Color.black ? _healthFillColor : _healthFillColor = new Color(0.6525454f, 0.9534668f, 0.9811321f, 1);

    }
    public Image HealthFillRight {
        get => _healthFillRight != null ? _healthFillRight : _healthFillRight = transform.Find("MainDisplay/Health/Background/Right").GetComponent<Image>();
    }
    public Image HealthFillLeft
    {
        get => _healthFillLeft != null ? _healthFillLeft : _healthFillLeft = transform.Find("MainDisplay/Health/Background/Left").GetComponent<Image>();
    }
    public Image HealthBackground
    {
        get => _healthBackground != null ? _healthBackground : _healthBackground = transform.Find("MainDisplay/Health/Background").GetComponent<Image>();
    }
    public Text HealthText {
        get => _healthText != null ? _healthText : _healthText = transform.Find("MainDisplay/Health/HealthText").GetComponent<Text>();
    }

    //Primary
    public GameObject PrimaryHolder {
        get => _primaryHolder != null ? _primaryHolder : _primaryHolder = transform.Find("MainDisplay/Weapons/Primary").gameObject;
    }
    public Image PrimaryGunIcon {
        get => _primaryGunIcon != null ? _primaryGunIcon : _primaryGunIcon = transform.Find("MainDisplay/Weapons/Primary/Icon").GetComponent<Image>();
    }
    public Text PrimaryAmmoCount {
        get => _primaryAmmoCount != null ? _primaryAmmoCount : _primaryAmmoCount = transform.Find("MainDisplay/Weapons/Primary/Icon/Text").GetComponent<Text>();
    }

    //Secondary
    public GameObject SecondaryHolder
    {
        get => _secondaryHolder != null ? _secondaryHolder : _secondaryHolder = transform.Find("MainDisplay/Weapons/Secondary").gameObject;
    }
    public Image SecondaryGunIcon
    {
        get => _secondaryGunIcon != null ? _secondaryGunIcon : _secondaryGunIcon = transform.Find("MainDisplay/Weapons/Secondary/Icon").GetComponent<Image>();
    }
    public Text SecondaryAmmoCount
    {
        get => _secondaryAmmoCount != null ? _secondaryAmmoCount : _secondaryAmmoCount = transform.Find("MainDisplay/Weapons/Secondary/Icon/Text").GetComponent<Text>();
    }

    //Objectives
    [SerializeField] protected GameObject ObjectiveDisplay {
        get => _objectiveDisplay != null ? _objectiveDisplay : _objectiveDisplay = transform.Find("MainDisplay/ObjectiveDisplay").gameObject;
    }
    [SerializeField] protected Text ObjectiveText {
        get => _objectiveText != null ? _objectiveText : _objectiveText = transform.Find("MainDisplay/ObjectiveDisplay/Text").GetComponent<Text>();
    }
    [SerializeField] protected GameObject ObjectiveMarker { 
        get => _objectiveMarker != null ? _objectiveMarker : _objectiveMarker = transform.Find("MainDisplay/ObjectiveMarker").gameObject;
    }
    [SerializeField] protected Text ObjectiveMarkerText {
        get => _objectiveMarkerText != null ? _objectiveMarkerText : _objectiveMarkerText = transform.Find("MainDisplay/ObjectiveMarker/Text").GetComponent<Text>();
    }

    //Input Display
    [SerializeField] protected GameObject InputDisplay {
        get => _inputDisplay != null ? _inputDisplay : _inputDisplay = transform.Find("MainDisplay/InputMessage").gameObject;
    }
    [SerializeField] protected Text InputText {
        get => _inputText != null ? _inputText : _inputText = transform.Find("MainDisplay/InputMessage/Text").GetComponent<Text>();
    }

    //Additional variables
    private IEnumerator currentFunction;

    //Additional Functions
    protected void UpdateAmmo(Text text, int current, int total) {
        text.text = current + "/" + total;
    }
    protected void UpdateIcon(Image icon, string iconPath) {
        //Makes sure the icon exists if it does set it, if not finds the "missing gun"-icon
        icon.sprite = Resources.Load<Sprite>(iconPath) != null ? Resources.Load<Sprite>(iconPath) : Resources.Load<Sprite>("GunIcons/Missing");
    }
    protected void UpdateObjective(string objective = "") {
        //Clears and updates the main display
        ObjectiveDisplay.SetActive(objective != "");
        ObjectiveText.text = objective;

        //Clears the side objective
        ObjectiveMarker.SetActive(false);
        ObjectiveMarkerText.text = "";

        //Stops the IEnumerator if it exists
        if(currentFunction != null)
            StopCoroutine(currentFunction);

        //Transfers into the side objective after a while.
        if (objective != ""){
            currentFunction = TransferObjective(3, objective);
            StartCoroutine(currentFunction);
        }
    }
    protected void UpdateObjectiveMarker(string objective = "") {
        //Clears the side objective
        ObjectiveMarker.SetActive(objective != "");
        ObjectiveMarkerText.text = objective;
    }
    protected void UpdateInput(string inputText = "") {
        InputText.text = inputText;
    }

    //Timed Functions
    protected IEnumerator TransferObjective(float timer, string objective) {
        yield return new WaitForSeconds(timer);

        //Turns off the main display
        ObjectiveDisplay.SetActive(false);
        ObjectiveText.text = "";

        //Updates the side objective display
        ObjectiveMarker.SetActive(true);
        ObjectiveMarkerText.text = objective;
    }
}
