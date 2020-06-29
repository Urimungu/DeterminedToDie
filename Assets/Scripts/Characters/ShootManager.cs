using UnityEngine;

public class ShootManager{

    /// <summary>
    /// Shoots for single types of shots like pistols, rifles, or snipers.
    /// </summary>
    /// <param name="data">The variables that are needed for the player to be able to shoot properly.</param>
    public static void ShootSingle(GameObject trail, PlayerController data) {
        //Adds the shooting inaccuracy/Spread to the gun
        var accuracy = new Vector3(Random.Range(-data.GunAccuracy, data.GunAccuracy), Random.Range(-data.GunAccuracy, data.GunAccuracy), 0);
        var newForward = data.ShootPoint.forward + accuracy;

        //Shows the bullet path
        Debug.DrawRay(data.ShootPoint.position, newForward * 20, Color.red);

        //Initializes the shot point
        var hitDistance = 20f;
        var hitPoint = data.ShootPoint.position + (newForward * hitDistance);

        //Raycasts forward to see if it hit anything
        if(Physics.Raycast(data.ShootPoint.position, newForward, out RaycastHit hit, 20, data.ShootMask)) {
            if(hit.collider != null) {
                hitDistance = hit.distance;
                hitPoint = hit.point;
            }
        }

        //Sets the Gun Trail
        trail.transform.position = data.ShootPoint.position + (newForward * (hitDistance / 2));
        trail.transform.LookAt(hitPoint);
        trail.transform.localScale = new Vector3(1, 1, hitDistance * 4);
    }
}
