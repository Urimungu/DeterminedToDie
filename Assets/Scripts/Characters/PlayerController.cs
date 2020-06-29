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
            ShootMechanics();
        }

        //Reloads the gun
        if(Input.GetKeyDown(KeyCode.R) && CurrentChamberAmmo < ChamberSize && CurrentAmmo > 0 && !_isRunning) {
            _isRunning = true;
            StartCoroutine(ReloadGun());
        }
    }

    //Controls the shooting mechanics
    private void ShootMechanics() {
        //Calculates the fire rate and ammo count
        _shootTimer = Time.time + FireRate;
        CurrentChamberAmmo--;

        //Creates the asthetics of damage being taken
        var trail = Instantiate(Resources.Load<GameObject>("FX/BulletTrail"));
        var damageText = Instantiate(Resources.Load<GameObject>("FX/DamageMarker"));

        //Calls the static scripts
        ShootManager.ShootSingle(trail, damageText, this);
        CameraController.RecoilCamera(GunRecoil, this);

        //Removes the markers after they were created
        StartCoroutine(RemoveBulletTrail(trail));
        StartCoroutine(RemoveTextMarker(damageText));

        //Updates the HUD if it isn't missing
        if(GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdateAmmo(CurrentChamberAmmo, CurrentAmmo);
    }

    //Reloads the gun
    IEnumerator ReloadGun() {
        yield return new WaitForSeconds(ReloadTime);
        var missingAmmo = Mathf.Clamp(ChamberSize - CurrentChamberAmmo, 0, CurrentAmmo);
        CurrentChamberAmmo += missingAmmo;
        CurrentAmmo -= missingAmmo;

        //Updates the HUD if it isn't missing
        if(GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdateAmmo(CurrentChamberAmmo, CurrentAmmo);
        _isRunning = false;
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

        //Updates the HUD if it isn't missing
        if(GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdateHealth(CurrentHealth, MaxHealth);
    }
}
