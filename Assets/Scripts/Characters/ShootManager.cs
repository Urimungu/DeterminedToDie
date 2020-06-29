using UnityEditorInternal;
using UnityEngine;

public class ShootManager{

    /// <summary>
    /// Shoots for single types of shots like pistols, rifles, or snipers.
    /// </summary>
    /// <param name="data">The variables that are needed for the player to be able to shoot properly.</param>
    public static void ShootSingle(GameObject trail, GameObject text, PlayerController data) {
        //Adds the shooting inaccuracy/Spread to the gun
        var accuracy = new Vector3(Random.Range(-data.GunAccuracy, data.GunAccuracy), Random.Range(-data.GunAccuracy, data.GunAccuracy), 0);
        if(data.MoveState == CharacterStats.MovementState.Aiming) 
            accuracy /= 3;
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

                //Hits an enemy and deals damage
                if(hit.collider.GetComponent<EnemyController>() != null) {
                    var damage = Random.Range(data.Damage - 10, data.Damage + 10);
                    hit.collider.GetComponent<EnemyController>().TakeDamage(damage);

                    //Sets the Damage Text
                    if(hit.collider.GetComponent<EnemyController>().CanMove) {
                        text.transform.position = hit.point;
                        text.transform.LookAt(data.transform.position);
                        text.transform.GetChild(0).GetComponent<TextMesh>().text = damage.ToString("00");

                        //Sets the Damage Marker Color
                        var color = Color.yellow;
                        if(damage < data.Damage - 5) color = Color.red;
                        else if(damage < data.Damage + 5) color = Color.white;

                        text.transform.GetChild(0).GetComponent<TextMesh>().color = color;
                    }
                }
            }
        }

        //Sets the Gun Trail
        trail.transform.position = data.ShootPoint.position + (newForward * (hitDistance / 2));
        trail.transform.LookAt(hitPoint);
        trail.transform.localScale = new Vector3(1, 1, hitDistance * 4);
    }
}
