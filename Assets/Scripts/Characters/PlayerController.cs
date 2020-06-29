using System.Collections;
using UnityEngine;

public class PlayerController : CharacterStats {

    //Variables
    private float _shootTimer;
    private bool _isRunning;

    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Sets the Game Manager Reference
        if(GameManager.Instance == null) return;
        GameManager.Instance.Player = this;

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

        Debug.DrawRay(lookPosition, PlayerCamera.transform.GetChild(0).forward * 20, Color.red);

        //Raycasts forward to see if it hit anything
        if(Physics.Raycast(lookPosition, PlayerCamera.transform.GetChild(0).forward, out RaycastHit hit, 20, ShootMask)) {
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
        bool mousePress = AutomaticWeapon ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        if(mousePress && Time.time > _shootTimer && CurrentChamberAmmo > 0) {
            _shootTimer = Time.time + FireRate;
            CurrentChamberAmmo--;
            var trail = Instantiate(Resources.Load<GameObject>("FX/BulletTrail"));
            StartCoroutine(RemoveBulletTrail(trail));

            ShootManager.ShootSingle(trail, this);
            CameraController.RecoilCamera(GunRecoil, this);
            GameManager.Instance.HUD.UpdateAmmo(CurrentChamberAmmo, CurrentAmmo);
        }

        //Reloads the gun
        if(Input.GetKeyDown(KeyCode.R) && CurrentChamberAmmo < ChamberSize && CurrentAmmo > 0 && !_isRunning) {
            _isRunning = true;
            StartCoroutine(ReloadGun());
        }
        
    }

    //Reloads the gun
    IEnumerator ReloadGun() {
        yield return new WaitForSeconds(ReloadTime);
        var missingAmmo = Mathf.Clamp(ChamberSize - CurrentChamberAmmo, 0, CurrentAmmo);
        CurrentChamberAmmo += missingAmmo;
        CurrentAmmo -= missingAmmo;
        GameManager.Instance.HUD.UpdateAmmo(CurrentChamberAmmo, CurrentAmmo);
        _isRunning = false;
    }

    //Removes the bullet trail once it is spawned in
    IEnumerator RemoveBulletTrail(GameObject trail) {
        yield return new WaitForSeconds(BulletTrailLifeTime);
        Destroy(trail);
    }

    /// <summary>
    /// Deal damage to the player.
    /// </summary>
    /// <param name="amount">The amount of damage that the player will take.</param>
    public void TakeDamage(float amount) {
        CurrentHealth = Mathf.Clamp(CurrentHealth - amount, 0, MaxHealth);
        GameManager.Instance.HUD.UpdateHealth(CurrentHealth, MaxHealth);
    }
}
