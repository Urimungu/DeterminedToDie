﻿using UnityEngine;

public class CameraController{

    /// <summary>
    /// Controls the camera so it follows the player in a Third Person Shooter feel.
    /// </summary>
    /// <param name="hor">Input needed to move the Camera left and right.</param>
    /// <param name="ver">Input needed to move the Camera up and down.</param>
    /// <param name="stats">Gets the Camera variables, such as height, distance, sensitivity, etc.</param>
    public static void FollowPlayer(float hor, float ver, PlayerController stats) {
        //Moves Camera
        RotateCamera(hor, stats);
        TiltCamera(ver, stats);
        CameraPositioning(stats);
    }

    //Positions the camera in the right place
    private static void CameraPositioning(PlayerController stats) {
        //Addes Horizontal offset
        var direction = stats.HoverRight ? 1 : -1;
        var newPos = stats.transform.position;
        newPos += (stats.transform.right * stats.CameraHorizontalOffset * direction);

        //Adds Vertical Offset
        newPos += (stats.transform.up * stats.CameraHeight);

        //Sets it to the camera
        stats.PlayerCamera.transform.position = newPos;

        //Makes the camera aim
        var newLookAt = stats.PlayerCamera.transform.position + (stats.PlayerCamera.transform.forward * stats.CameraViewDistance);
        stats.PlayerCamera.transform.Find("Camera").LookAt(newLookAt);
    }

    //Rotates the player left and right
    private static void RotateCamera(float hor, PlayerController stats) {
        //Rotates the camera and the player
        stats.transform.Rotate(new Vector3(0, stats.HorizontalSensitivity * hor, 0));
        stats.PlayerCamera.transform.rotation = stats.transform.rotation;
    }

    //Checks to see if there is anything behind the camera so it doesn't clip through it
    private static void TiltCamera(float ver, PlayerController stats) {
        //Sets references
        var cam = stats.PlayerCamera.transform.Find("Camera");
        Vector3 newPos = cam.transform.localPosition;

        //Makes sure there is nothing behind the player
        var currentRadius = stats.CameraDistance;
        var direction = (cam.transform.position - stats.PlayerCamera.transform.position).normalized;

        //Checks with raycast
        if(Physics.Raycast(stats.PlayerCamera.transform.position, direction, out RaycastHit hit, stats.CameraDistance, stats.CameraMask)) {
            if(hit.collider != null && hit.distance >= stats.CameraMinDistance) {
                currentRadius = hit.distance;
            }
        }

        //Circle Equation
        Vector3 CircleEquation(Vector3 vect, float radius) {
            float theta = Mathf.Clamp(Mathf.Atan((vect.y - (ver * stats.VerticalSensitivity)) / -vect.z), -0.7f, 1.5f);
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
