using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraController{

    /// <summary>
    /// Controls the camera so it follows the player in a Third Person Shooter feel.
    /// </summary>
    /// <param name="hor">Input needed to move the Camera left and right.</param>
    /// <param name="ver">Input needed to move the Camera up and down.</param>
    /// <param name="stats">Gets the Camera variables, such as height, distance, sensitivity, etc.</param>
    public static void FollowPlayer(float hor, float ver, CharacterFunctions stats) {
        //Moves Camera
        RotateCamera(hor, stats);
        TiltCamera(ver, stats);
        CameraPositioning(stats);

        //Sets the Field of View
        var currentFOV = stats.CameraFOV;
        if(stats.MoveState == CharacterStats.MovementState.Aiming) currentFOV = stats.AimingFOV;
        else if(stats.MoveState == CharacterStats.MovementState.Running) currentFOV = stats.RunningFOV;

        //Smooth transition
        if(currentFOV != stats.CurrentFOV) 
            stats.CurrentFOV = Mathf.SmoothStep(stats.CurrentFOV, currentFOV, stats.FOVStep);

        stats.PlayerCamera.transform.Find("Camera").GetComponent<Camera>().fieldOfView = stats.CurrentFOV;
        
    }

    //Positions the camera in the right place
    public static void CameraPositioning(CharacterFunctions stats) {
        //Addes Horizontal offset
        var direction = stats.HoverRight ? 1 : -1;
        var newPos = stats.transform.position;
        var horOffset = stats.CameraHorizontalOffset;
        if(stats.MoveState == CharacterStats.MovementState.Running) horOffset = stats.RunCamHorizontalOffset;

        //Smooth transition from walking to running
        if(horOffset != stats.CurrentHorizontalOffset)
            stats.CurrentHorizontalOffset = Mathf.SmoothStep(stats.CurrentHorizontalOffset, horOffset, stats.HorOffsetStep);
        

        newPos += (stats.transform.right * stats.CurrentHorizontalOffset * direction);

        //Adds Vertical Offset
        newPos += (stats.transform.up * stats.CameraHeight);

        //Sets it to the camera
        stats.PlayerCamera.transform.position = newPos;

        //Makes the camera aim
        var newLookAt = stats.PlayerCamera.transform.position;
        stats.PlayerCamera.transform.Find("Camera").LookAt(newLookAt);
    }

    //Rotates the player left and right
    public static void RotateCamera(float hor, CharacterFunctions stats) {
        //Depends if the player is aiming down the sights
        var horizontalSensitivity = stats.MoveState == CharacterStats.MovementState.Aiming ? stats.HorizontalAimSensitivity : stats.HorizontalSensitivity;

        //Rotates the camera and the player
        stats.transform.Rotate(new Vector3(0, horizontalSensitivity * hor, 0));
        stats.PlayerCamera.transform.rotation = stats.transform.rotation;
    }

    /// <summary>
    /// Adds Recoil to the camera. Making the camera slightly move up every time the function is called.
    /// </summary>
    /// <param name="recoil">The amount of recoild or movement the gun will have, or the camera will move.</param>
    /// <param name="stats">The Player Controller or character that this will affect.</param>
    public static void RecoilCamera(float recoil, CharacterFunctions stats) {
        TiltCamera(recoil, stats);
    }

    //Checks to see if there is anything behind the camera so it doesn't clip through it
    private static void TiltCamera(float ver, CharacterFunctions stats) {
        //Sets references
        var cam = stats.PlayerCamera.transform.Find("Camera");
        Vector3 newPos = cam.transform.localPosition;

        //Makes sure there is nothing behind the player
        var direction = (cam.transform.position - stats.PlayerCamera.transform.position).normalized;

        //Chooses the camera distance
        var curRadius = stats.CameraDistance;
        if(stats.MoveState == CharacterStats.MovementState.Aiming) curRadius = stats.AimCameraDistance;
        else if(stats.MoveState == CharacterStats.MovementState.Running) curRadius = stats.RunCameraDistance;

        //Smooth transition between the movement
        if(curRadius != stats.CurrentCameraRadius)
            stats.CurrentCameraRadius = Mathf.SmoothStep(stats.CurrentCameraRadius, curRadius, stats.RadiusStep);

        var currentRadius = stats.CurrentCameraRadius;

        //Checks with raycast
        if(Physics.Raycast(stats.PlayerCamera.transform.position, direction, out RaycastHit hit, curRadius, stats.CameraMask)) {
            if(hit.collider != null && hit.distance >= stats.CameraMinDistance) {
                currentRadius = hit.distance - 0.1f;
            }
        }

        var verticalSensitivity = stats.MoveState == CharacterStats.MovementState.Aiming ? stats.VerticalAimSensitivity : stats.VerticalSensitivity;

        //Circle Equation
        Vector3 CircleEquation(Vector3 vect, float radius) {
            float theta = Mathf.Clamp(Mathf.Atan((vect.y - (ver * verticalSensitivity)) / -vect.z), -0.7f, 1.5f);
            vect.z = -radius * Mathf.Cos(theta);
            vect.y = radius * Mathf.Sin(theta);
            return vect;
        }

        //Calculates the tilt movement
        newPos = CircleEquation(newPos, currentRadius);

        //Sets the positions and fixes the Child position
        stats.PlayerCamera.transform.Find("Camera").localPosition = newPos;
    }
}
