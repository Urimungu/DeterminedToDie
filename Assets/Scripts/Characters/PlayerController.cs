using UnityEngine;

public class PlayerController : CharacterStats {

    private void Update() {
        if (CanMove){
            //Makes the player move
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");
            Movement.Walk(horizontal, vertical, this);

            //Makes the camera move
            float mouseHorizontal = Input.GetAxisRaw("Mouse X");
            float mouseVertical = Input.GetAxisRaw("Mouse Y");
            CameraController.FollowPlayer(mouseHorizontal, mouseVertical, this);

            //Locks the Cursor
            if(Input.GetKeyDown(KeyCode.Escape)) {
                if(Cursor.lockState == CursorLockMode.Confined) {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                } else {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
        }
    }

}
