using UnityEngine.UI;
using UnityEngine;

public class HUDManager : MonoBehaviour{

    [Header("References")]
    [SerializeField] private Image _crossHair;
    [SerializeField] private Image _healthFill;
    [SerializeField] private Image _healthBackground;
    [SerializeField] private Text _healthText;

    //Variables
    private Color _ogHPBarColor;

    #region Properties
    //Sets all of the Private variables in case they weren't ever set in the first place
    public Image CrossHair {
        get {
            if(_crossHair == null) _crossHair = transform.Find("CrossHairs").GetComponent<Image>();
            return _crossHair;
        }
    }

    public Image HealthFill{
        get {
            if(_healthFill == null) _healthFill = transform.Find("Health/Background/Fill").GetComponent<Image>();
            return _healthFill;
        }
    }

    public Image HealthBackground{
        get {
            if(_healthBackground == null) _healthBackground = transform.Find("Health/Background").GetComponent<Image>();
            return _healthBackground;
        }
    }

    public Text HealthText {
        get {
            if(_healthText == null) _healthText = transform.Find("Health/HealthText").GetComponent<Text>();
            return _healthText;
        }
    }
    #endregion

    private void Awake() {
        _ogHPBarColor = HealthFill.color;

        //Returns if there isn't any GameManager on the field
        if(GameManager.Instance == null) return;

        //Sets itself as a reference for the Game Manager
        GameManager.Instance.HUD = this;
    }

    /// <summary>
    /// Updates the display for player health in User Interface. 
    /// </summary>
    /// <param name="amount">The amount of health that the player has remaining.</param>
    /// <param name= "max">The max amount of health that the player could have.</param>
    public void UpdateHealth(float amount, float max) {
        //Shows the images if the player has lost health and puts them away if they are maxed out
        HealthFill.gameObject.SetActive(amount < max);
        HealthBackground.gameObject.SetActive(amount < max);
        HealthText.gameObject.SetActive(amount < max);

        //Displays the amount of health to the player through image and text
        HealthFill.fillAmount = amount / max;
        HealthText.text = amount.ToString("00") + " / " + max.ToString("00");

        //Changes the color from base color to red
        _healthFill.color = (_ogHPBarColor * (amount / max)) + (Color.red * ( 1 - (amount / max)));
    }

}
