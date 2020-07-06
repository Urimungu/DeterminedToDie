using UnityEngine.UI;
using UnityEngine;

public class HUDManager : MonoBehaviour{

    [Header("References")]
    [SerializeField] private Image _crossHair;
    [SerializeField] private Image _healthFill;
    [SerializeField] private Image _healthBackground;
    [SerializeField] private Text _healthText;

    [Header("Weapons")]
    [SerializeField] private Image _primaryGunIcon;
    [SerializeField] private Text _primaryAmmoCount;
    [SerializeField] private Image _secondaryGunIcon;
    [SerializeField] private Text _secondaryAmmoCount;

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

    public Text PrimaryAmmoCount {
        get {
            if(_primaryAmmoCount == null) _primaryAmmoCount = transform.Find("WeaponInfo/WeaponIcon/Text").GetComponent<Text>();
            return _primaryAmmoCount;
        }
    }

    public Image PrimaryGunIcon {
        get {
            if(_primaryGunIcon == null) _primaryGunIcon = transform.Find("WeaponInfo/WeaponIcon").GetComponent<Image>();
            return _primaryGunIcon;
        }
    }

    public Image SecondaryGunIcon {
        get {
            if(_secondaryGunIcon == null)
                _secondaryGunIcon = transform.Find("WeaponInfo/SecondaryWeapon").GetComponent<Image>();
            return _secondaryGunIcon;
        }
    }

    public Text SecondaryAmmoCount {
        get {
            if(_secondaryAmmoCount == null)
                _secondaryAmmoCount = transform.Find("WeaponInfo/SecondaryWeapon/Text").GetComponent<Text>();
            return _secondaryAmmoCount;
        }
    }

    #endregion

    private void Awake() {
        _ogHPBarColor = HealthFill.color;

        //Returns if there isn't any GameManager on the field
        if(GameManager.Instance == null) return;

        //Sets itself as a reference for the Game Manager
        GameManager.Instance.HUD = this;

        //Updates the player health
        if(GameManager.Instance.Player != null) {
            UpdatePrimaryIcon(GameManager.Instance.Player.Primary.IconPath);
            UpdateSecondaryIcon(GameManager.Instance.Player.Secondary.IconPath);
            UpdateHealth(GameManager.Instance.Player.CurrentHealth, GameManager.Instance.Player.MaxHealth);
            UpdatePrimaryAmmo(GameManager.Instance.Player.PrimaryChamberAmmo, GameManager.Instance.Player.CurrentPrimaryAmmo);
            UpdateSecondaryAmmo(GameManager.Instance.Player.SecondaryChamberAmmo, GameManager.Instance.Player.CurrentSecondaryAmmo);
        }
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

    /// <summary>
    /// Updates the gun icon for the primary weapon on the UI.
    /// </summary>
    /// <param name="iconPath">The path in the resources that the image is at.</param>
    public void UpdatePrimaryIcon(string iconPath) {
        //Checks to make sure it exists first
        if(Resources.Load<Sprite>(iconPath) != null) {
            PrimaryGunIcon.sprite = Resources.Load<Sprite>(iconPath);
            return;
        }
        //If it doesn't exist place a place holder
        PrimaryGunIcon.sprite = Resources.Load<Sprite>("GunIcons/Missing");
    }

    /// <summary>
    /// Updates the gun icon for the Secodnary weapon on the UI.
    /// </summary>
    /// <param name="iconPath">The path in the resources that the image is at.</param>
    public void UpdateSecondaryIcon(string iconPath) {
        //Checks to make sure it exists first
        if(Resources.Load<Sprite>(iconPath) != null) {
            SecondaryGunIcon.sprite = Resources.Load<Sprite>(iconPath);
            return;
        }
        //If it doesn't exist place a place holder
        SecondaryGunIcon.sprite = Resources.Load<Sprite>("GunIcons/Missing");
    }

    /// <summary>
    /// Updates that amount of ammo that is displayed on the screen for the primary weapon.
    /// </summary>
    /// <param name="chamberAmmo">The amount of bullets that the player has in the chamber, and is ready to shoot.</param>
    /// <param name="ammoCount">The ammount of ammo they have that they can use by refilling and reloading.</param>
    public void UpdatePrimaryAmmo(int chamberAmmo, int ammoCount) {
        //Changes the Text
        PrimaryAmmoCount.text = chamberAmmo + "/" + ammoCount;
    }

    /// <summary>
    /// Updates that amount of ammo that is displayed on the screen for the secondary weapon.
    /// </summary>
    /// <param name="chamberAmmo">The amount of bullets that the player has in the chamber, and is ready to shoot.</param>
    /// <param name="ammoCount">The ammount of ammo they have that they can use by refilling and reloading.</param>
    public void UpdateSecondaryAmmo(int chamberAmmo, int ammoCount) {
        //Changes the Text
        SecondaryAmmoCount.text = chamberAmmo + "/" + ammoCount;
    }

}
