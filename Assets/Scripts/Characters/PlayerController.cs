using System.Collections;
using UnityEngine;

public class PlayerController : CharacterStats {

    //Variables
    private float _shootTimer;
    private bool _isReloading;
    private bool _isShooting;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Sets the Game Manager Reference
        if(GameManager.Instance == null) return;
        GameManager.Instance.Player = this;

        //Fills up Ammo
        Primary = GameManager.Instance.WeaponDatabase.GunCatalog[3];
        PrimaryChamberAmmo = Primary.ChamberSize;
        Secondary = GameManager.Instance.WeaponDatabase.GunCatalog[0];
        SecondaryChamberAmmo = Secondary.ChamberSize;

    }

    private void Update() {
        if (CanMove){
            //Makes the player move
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Movement.Move(horizontal, vertical, this);

            //Makes the camera move
            float mouseHorizontal = Input.GetAxisRaw("Mouse X");
            float mouseVertical = Input.GetAxisRaw("Mouse Y");
            CameraController.FollowPlayer(mouseHorizontal, mouseVertical, this);

            //Buttons pressing
            KeyPressing(vertical);

            //Controls the Gun Movement
            GunController();
        }
    }

    //Controls the gun movement
    private void GunController() {
        //Position of the start of the radius
        var lookPosition = PlayerCamera.transform.GetChild(0).position + (PlayerCamera.transform.GetChild(0).forward * CameraDistance);
        var lookPoint = lookPosition + (PlayerCamera.transform.GetChild(0).forward * 10);

        //Raycasts forward to see if it hit anything
        if(Physics.Raycast(lookPosition, PlayerCamera.transform.GetChild(0).forward, out RaycastHit hit, 500, ShootMask)) {
            if(hit.collider != null) {
                lookPoint = hit.point;
            }
        }

        //Looks at the camera aiming position
        GunHolder.LookAt(lookPoint);
    }

    //The Keys that the player presses to make the player move
    private void KeyPressing(float vertical) {
        //Locking and Unlocking the Cursor
        if(Input.GetKeyDown(KeyCode.Escape)) {
            if(Cursor.lockState == CursorLockMode.Confined) {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            } else {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }

        //Running, Walking, and Crouching
        if(Input.GetMouseButton(1))
            MoveState = MovementState.Aiming;
        else if(Input.GetKey(KeyCode.LeftShift) && vertical > 0.1f)
            MoveState = MovementState.Running;
        else if(Input.GetKey(KeyCode.LeftControl) && CheckIfGrounded)
            MoveState = MovementState.Crouching;
        else
            MoveState = MovementState.Walking;

        //Jumping
        if(Input.GetButton("Jump"))
            Movement.Jump(this);

        //If the player is running and cannot handle the gun
        if(MoveState == MovementState.Running) return;

        //Shoot
        bool mousePress = Primary.AutomaticWeapon ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        if(mousePress && Time.time > _shootTimer && PrimaryChamberAmmo > 0) {
            ShootMechanics();
        }

        //Reloads the gun
        if(Input.GetKeyDown(KeyCode.R) && PrimaryChamberAmmo < Primary.ChamberSize && CurrentPrimaryAmmo > 0 && !_isReloading) {
            _isReloading = true;
            StartCoroutine(ReloadGun());
        }

        //Switches Weapon
        if(Input.GetKeyDown(KeyCode.Tab) && !_isReloading) {
            _shootTimer = Time.time + Secondary.StowSpeed;
            //Switches gun
            var tempGun = Primary;
            Primary = Secondary;
            Secondary = tempGun;

            //Switches Ammo count
            var tempAmmo = CurrentPrimaryAmmo;
            var tempChamber = PrimaryChamberAmmo;
            CurrentPrimaryAmmo = CurrentSecondaryAmmo;
            PrimaryChamberAmmo = SecondaryChamberAmmo;
            CurrentSecondaryAmmo = tempAmmo;
            SecondaryChamberAmmo = tempChamber;

            //Switches Display
            if(GameManager.Instance != null && GameManager.Instance.HUD != null) {
                GameManager.Instance.HUD.UpdatePrimaryIcon(Primary.IconPath);
                GameManager.Instance.HUD.UpdatePrimaryAmmo(PrimaryChamberAmmo, CurrentPrimaryAmmo);
                GameManager.Instance.HUD.UpdateSecondaryIcon(Secondary.IconPath);
                GameManager.Instance.HUD.UpdateSecondaryAmmo(SecondaryChamberAmmo, CurrentSecondaryAmmo);
            }
        }
    }

    //Controls the shooting mechanics
    private void ShootMechanics() {
        //Calculates the fire rate and ammo count
        _shootTimer = Time.time + Primary.FireRate;
        PrimaryChamberAmmo--;

        //Calls the static scripts
        if(!_isShooting) {
            _isShooting = true;
            StartCoroutine(ShootGun());
        }

        CameraController.RecoilCamera(Primary.GunRecoil, this);

        //Updates the HUD if it isn't missing
        if(GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdatePrimaryAmmo(PrimaryChamberAmmo, CurrentPrimaryAmmo);
    }

    IEnumerator ShootGun() {
        for(int i = 0; i < Primary.BulletsPerShot; i++) {
            //Creates the asthetics of damage being taken
            var trail = Instantiate(Resources.Load<GameObject>("FX/BulletTrail"));
            var damageText = Instantiate(Resources.Load<GameObject>("FX/DamageMarker"));

            //Applies the Shots
            RaycastHit hit = ShootManager.ShootSingle(trail, damageText, this);
            yield return new WaitForSeconds(Primary.TimePerShot);

            if (hit.collider != null) {
                //Sets the Sparks
                string bloodOrSparks = hit.collider.GetComponent<EnemyController>() != null ? "FX/Blood" : "FX/Sparks";

                //Sets the position of Sparks
                var effects = Instantiate(Resources.Load<GameObject>(bloodOrSparks));
                effects.transform.position = hit.point;
                effects.transform.rotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);
                effects.GetComponent<ParticleSystem>().Play();

                //Removes sparks
                StartCoroutine(RemoveBulletTrail(effects, 0.25f));
            }

            //Removes the markers after they were created
            StartCoroutine(RemoveBulletTrail(trail, Primary.BulletTrailLifeTime));
            StartCoroutine(RemoveTextMarker(damageText));
        }
        _isShooting = false;
    }

    //Reloads the gun
    IEnumerator ReloadGun() {
        yield return new WaitForSeconds(Primary.ReloadTime);
        var missingAmmo = Mathf.Clamp(Primary.ChamberSize - PrimaryChamberAmmo, 0, CurrentPrimaryAmmo);
        PrimaryChamberAmmo += missingAmmo;
        CurrentPrimaryAmmo -= missingAmmo;

        //Updates the HUD if it isn't missing
        if(GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdatePrimaryAmmo(PrimaryChamberAmmo, CurrentPrimaryAmmo);
        _isReloading = false;
    }

    IEnumerator RemoveTextMarker(GameObject marker) {
        //Moves the text up until it is destroyed
        for(int i = 0; i < 20; i++) {
            yield return new WaitForSeconds(0.01f);
            marker.transform.position += new Vector3(0, 0.1f, 0);
            var color = marker.transform.GetChild(0).GetComponent<TextMesh>().color;
            color.a -= 0.05f;
            marker.transform.GetChild(0).GetComponent<TextMesh>().color = color;
        }

        Destroy(marker);
    }

    //Removes the bullet trail once it is spawned in
    IEnumerator RemoveBulletTrail(GameObject trail, float timer) {
        yield return new WaitForSeconds(timer);
        Destroy(trail);
    }

    /// <summary>
    /// Deal damage to the player.
    /// </summary>
    /// <param name="amount">The amount of damage that the player will take.</param>
    public void TakeDamage(float amount) {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);

        //Updates the HUD if it isn't missing
        if(GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdateHealth(CurrentHealth, MaxHealth);
    }
}
