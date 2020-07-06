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
            stats.RigidBody.AddForce((moveForward + moveRight).normalized * stats.AirSpeed * 100);
            return;
        }

        //Chooses what speed the player moves in
        float tempSpeed = 0;
        switch(stats.MoveState) {
            //Aiming, Walking, Running, Crouching Speeds
            case CharacterStats.MovementState.Aiming:    tempSpeed = stats.AimingWalkSpeed;break;
            case CharacterStats.MovementState.Walking:   tempSpeed = stats.WalkingSpeed;   break;
            case CharacterStats.MovementState.Running:   tempSpeed = stats.RunningSpeed;   break;
            case CharacterStats.MovementState.Crouching: tempSpeed = stats.CrouchingSpeed; break;
        }

        //Removes Diagnol speed gain and adds back in it's vertical velocity
        Vector3 newMovement = (moveForward + moveRight).normalized * tempSpeed;
        newMovement.y = stats.RigidBody.velocity.y;

        //Sends the Rigidbody the movement
        stats.RigidBody.velocity = newMovement;
    }

    /// <summary>
    /// Moves the Character into the position that has been indicated.
    /// </summary>
    /// <param name="movePos">The target position that the character will move towards.</param>
    /// <param name="stats">The stats needed to plug in the missing variables of movement speed and stuff.</param>
    public static void Move(Vector3 movePos, EnemyController stats) {
        //Determines the direction that the character has to move towards
        var newDir = (movePos - stats.transform.position).normalized;
        newDir.y = stats.EnemyRigidBody.velocity.y;

        //Turns the player towards the direction that they are moving
        var lookDir = stats.transform.position + newDir;
        stats.transform.LookAt(lookDir);

        //Sets all the movement into the character
        stats.EnemyRigidBody.velocity = newDir * stats.WalkingSpeed;
    }

    /// <summary>
    /// Controls the way the player jumps and how those dynamics work.
    /// </summary>
    /// <param name="stats">Variables needed for the movement speed, walking speed, running speed, etc.</param>
    public static void Jump(PlayerController stats) {
        //If in the air then don't read the rest of the function
        if(!stats.CheckIfGrounded) return;

        //Makes the player jump
        var preVel = stats.RigidBody.velocity;
        preVel.y += stats.JumpForce;
        stats.RigidBody.velocity = preVel;
        
    }
}
