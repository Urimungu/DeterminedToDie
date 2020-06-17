using UnityEngine;

public class Movement{

    /// <summary>
    /// Controls the way the player moves.
    /// </summary>
    /// <param name="hor">Input needed to control player moving from side to side.</param>
    /// <param name="ver">Input needed to control player moving forward and backwards.</param>
    /// <param name="stats">Variables needed for the movement speed, walking speed, running speed, etc.</param>
    public static void Walk(float hor, float ver, PlayerController stats) {
        //Gets the input for moving forward and moving right
        var moveForward = stats.transform.forward * ver;
        var moveRight = stats.transform.right * hor;

        //Removes Diagnol speed gain
        Vector3 newMovement = (moveForward + moveRight).normalized * stats.WalkingSpeed;

        //Sends the Rigidbody the movement
        stats.RigidBody.velocity = newMovement;
    }
}
