using UnityEngine;
using UnityEngine.Experimental.UIElements;

public class PlayerController : CharacterStats {

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
        }
    }

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

    }
}
