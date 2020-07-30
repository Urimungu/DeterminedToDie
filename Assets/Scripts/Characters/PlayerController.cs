using UnityEngine;

public class PlayerController : CharacterFunctions {

    private void Awake() {
        //Sets the Game Manager Reference
        if(GameManager.Instance == null) return;
        GameManager.Instance.Player = this;
    }

    private void Update() {
        //Calls everything if it can recieve input from the player
        if (CanRecieveInput)
        {
            PlayerMovement();
            CameraMovement();
            GunHandeling();
        }
        else if(!CanRecieveInput && _isDead){
            //Looks at player as it flies away
            PlayerCamera.transform.GetChild(0).LookAt(Corpse.transform.position);
            PlayerCamera.transform.position += new Vector3(0, 1, 0) * Time.deltaTime;
        }

        //Handles Animations
        ControlAnimations();
    }

    //Player input for the movement
    private void PlayerMovement() {
        //Stops if the player cannot move
        if (!CanMove) return;

        //Moves the Gun
        GunController();

        //Moves the player
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Movement.Move(horizontal, vertical, this);

        //Changes Movement State
        if (Input.GetMouseButton(1)) 
            MoveState = MovementState.Aiming;
        else if (Input.GetKey(KeyCode.LeftShift) && vertical > 0.1f) 
            MoveState = MovementState.Running;
        else if (Input.GetKey(KeyCode.LeftControl) && CheckIfGrounded) 
            MoveState = MovementState.Crouching;
        else if (MoveState != MovementState.Walking) 
            MoveState = MovementState.Walking;

        //Jumps
        if (Input.GetButton("Jump")) Movement.Jump(this);
    }

    //Player input for the camera
    private void CameraMovement() {
        //Stops if the player cannot move their camera
        if (!CanLook) return;

        //Moves the Camera
        float mouseHorizontal = Input.GetAxisRaw("Mouse X");
        float mouseVertical = Input.GetAxisRaw("Mouse Y");
        CameraController.FollowPlayer(mouseHorizontal, mouseVertical, this);
    }

    //Player input for the gun movement
    private void GunHandeling() {
        //Stops if the player cannot use their gun
        if (!CanShoot) return;

        //Shoots the Gun
        bool mousePress = Primary.AutomaticWeapon ? Input.GetMouseButton(0) : Input.GetMouseButtonDown(0);
        if (mousePress && Time.time > _shootTimer && Primary.ChamberAmmo > 0) {
            StartCoroutine(ShootGun());
            UpdatePrimary();
        }
        
        //Reloads the gun
        if (Input.GetKeyDown(KeyCode.R) && Primary.ChamberAmmo < Primary.ChamberSize && Primary.TotalAmmo > 0 && !_isReloading)
            StartCoroutine(ReloadGun());

        //Drops Primary
        if (Input.GetKeyDown(KeyCode.Q) && Primary.ID != 0)
            DropGun();

        //Swaps primary and secondary
        if (Input.GetKeyDown(KeyCode.Tab) && !_isReloading && Primary.ID != 0 && Secondary.ID != 0){
            SwapWeapons();
            UpdateGunDisplay();
        }
        
    }

    //Controls the Animations
    private void ControlAnimations() {
        var newVel = transform.InverseTransformDirection(PlayerRigidbody.velocity);
        Anim.SetFloat("Speed", new Vector2(newVel.x, newVel.z).magnitude);
        Anim.SetFloat("SpeedX", newVel.x);
        Anim.SetFloat("SpeedZ", newVel.z);
        Anim.SetFloat("VelocityY", newVel.y);
    }
}
