using UnityEngine;

public class GunPickUp : Interactable {

    [Header("Specific Variables")]
    [SerializeField] protected int gunID = 0;
    [SerializeField] protected GunType gun;
    [SerializeField] protected bool initialize = true;

    private void Start(){
        InteractDeclare = PickUp;

        if (gunID != 0 && initialize)
            gun = GameManager.Instance.WeaponDatabase.GetGun(gunID);
        
    }

    public void UpdateAmmo(GunType newGun) {
        initialize = false;
        gun = newGun;
    }

    protected void PickUp(CharacterFunctions player) {
        //Adds a Gun to the player hand
        player.PickUpGun(gun);

        //After it's been picked up it destroys itself
        Destroy(gameObject);
    }

}
