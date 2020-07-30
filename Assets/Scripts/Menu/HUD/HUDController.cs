using UnityEngine;

public class HUDController : HUDFunctions{

    private void Awake(){

        //Clears objectives and other things
        UpdateObjective();
        UpdateInput();

        //Returns if there isn't any GameManager on the field
        if (GameManager.Instance == null) return;

        //Sets itself as a reference for the Game Manager
        GameManager.Instance.HUD = this;

        //Updates the player health
        if (GameManager.Instance.Player != null){
            UpdateWeapons(GameManager.Instance.Player.Primary, GameManager.Instance.Player.Secondary);
            UpdateHealth(GameManager.Instance.Player.CurrentHealth, GameManager.Instance.Player.MaxHealth);
        }
    }

    /// <summary>
    /// Updates the Objective display in the Heads Up Display (HUD)
    /// </summary>
    /// <param name="text">The message that will be displayed.</param>
    public void UpdateObjectiveDisplay(string text = "") {
        UpdateObjective(text);
    }

    /// <summary>
    /// Updates the Heads up Display (HUD) when the player can interact with something.
    /// </summary>
    /// <param name="text">The message that will be displayed</param>
    public void UpdateInputDisplay(string text = "") {
        UpdateInput(text);
    }

    /// <summary>
    /// Updates the display for player health in User Interface. 
    /// </summary>
    /// <param name="amount">The amount of health that the player has remaining.</param>
    /// <param name= "max">The max amount of health that the player could have.</param>
    public void UpdateHealth(float amount, float max){
        //Initializes values
        var percent = amount / max;
        var barColor = HealthFillColor;
        var newColor = (barColor * percent) + (Color.red * (1 - percent));

        //Shows the images if the player has lost health and puts them away if they are maxed out
        HealthHolder.SetActive(amount < max);

        //Displays the amount of health to the player through image and text
        HealthFillRight.fillAmount = percent;
        HealthFillLeft.fillAmount = percent;
        HealthText.text = amount.ToString("00") + " / " + max.ToString("00");

        //Changes the color from base color to red
        HealthFillLeft.color = newColor;
        HealthFillRight.color = newColor;
    }

    /// <summary>
    /// Updates every aspect of both the primary weapon and the secondary weapon.
    /// </summary>
    /// <param name="primary">The primary weapon that will be reflected in the HUD.</param>
    /// <param name="secondary">The secondary weapon that will be reflected in the HUD.</param>
    public void UpdateWeapons(GunType primary, GunType secondary){
        //Updates Primary
        UpdatePrimary(primary);

        //Updates Secondary
        UpdateSecondary(secondary);

    }

    /// <summary>
    /// Updates every aspect of the Primary Weapon
    /// </summary>
    /// <param name="gun">The primary weapon that will be referenced by the HUD</param>
    public void UpdatePrimary(GunType gun) {
        PrimaryHolder.SetActive(gun.ID != 0);
        UpdatePrimaryIcon(gun.IconPath);
        UpdatePrimaryAmmo(gun.ChamberAmmo, gun.TotalAmmo);
    }

    /// <summary>
    /// Updates every aspect of the Secondary Weapon
    /// </summary>
    /// <param name="gun">The secondary weapon that will be referenced by the HUD</param>
    public void UpdateSecondary(GunType gun) {
        SecondaryHolder.SetActive(gun.ID != 0);
        UpdateSecondaryIcon(gun.IconPath);
        UpdateSecondaryAmmo(gun.ChamberAmmo, gun.TotalAmmo);
    }

    /// <summary>
    /// Updates the gun icon for the primary weapon on the UI.
    /// </summary>
    /// <param name="iconPath">The path in the resources that the image is at.</param>
    public void UpdatePrimaryIcon(string iconPath){
        UpdateIcon(PrimaryGunIcon, iconPath);
    }

    /// <summary>
    /// Updates that amount of ammo that is displayed on the screen for the primary weapon.
    /// </summary>
    /// <param name="chamberAmmo">The amount of bullets that the player has in the chamber, and is ready to shoot.</param>
    /// <param name="ammoCount">The ammount of ammo they have that they can use by refilling and reloading.</param>
    public void UpdatePrimaryAmmo(int chamberAmmo, int ammoCount)
    {
        UpdateAmmo(PrimaryAmmoCount, chamberAmmo, ammoCount);
    }

    /// <summary>
    /// Updates the gun icon for the Secodnary weapon on the UI.
    /// </summary>
    /// <param name="iconPath">The path in the resources that the image is at.</param>
    public void UpdateSecondaryIcon(string iconPath){
        UpdateIcon(SecondaryGunIcon, iconPath);
    }

    /// <summary>
    /// Updates that amount of ammo that is displayed on the screen for the secondary weapon.
    /// </summary>
    /// <param name="chamberAmmo">The amount of bullets that the player has in the chamber, and is ready to shoot.</param>
    /// <param name="ammoCount">The ammount of ammo they have that they can use by refilling and reloading.</param>
    public void UpdateSecondaryAmmo(int chamberAmmo, int ammoCount){
        UpdateAmmo(SecondaryAmmoCount, chamberAmmo, ammoCount);
    }

}
