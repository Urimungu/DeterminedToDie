using UnityEngine;

public class Movement{

    /// <summary>
    /// Controls the way the player walks.
    /// </summary>
    /// <param name="hor">Input needed to control player moving from side to side.</param>
    /// <param name="ver">Input needed to control player moving forward and backwards.</param>
    /// <param name="stats">Variables needed for the movement speed, walking speed, running speed, etc.</param>
    public static void Move(float hor, float ver, PlayerController stats) {
        //Gets the input for moving forward and moving right
        var moveForward = stats.transform.forward * ver;
        var moveRight = stats.transform.right * hor;

        if(!stats.CheckIfGrounded) {
            stats.PlayerRigidbody.AddForce((moveForward + moveRight).normalized * stats.AirSpeed * 100);
            return;
        }

        //Removes Diagnol speed gain and adds back in it's vertical velocity
        Vector3 newMovement = (moveForward + moveRight).normalized * stats.CurrentMovementSpeed;
        newMovement.y = stats.PlayerRigidbody.velocity.y;

        //Sends the Rigidbody the movement
        stats.PlayerRigidbody.velocity = newMovement;
    }

    /// <summary>
    /// Controls the way the player jumps and how those dynamics work.
    /// </summary>
    /// <param name="stats">Variables needed for the movement speed, walking speed, running speed, etc.</param>
    public static void Jump(PlayerController stats) {
        //If in the air then don't read the rest of the function
        if(!stats.CheckIfGrounded) return;

        //Makes the player jump
        var preVel = stats.PlayerRigidbody.velocity;
        preVel.y += stats.JumpForce;
        stats.PlayerRigidbody.velocity = preVel;
        
    }
}
