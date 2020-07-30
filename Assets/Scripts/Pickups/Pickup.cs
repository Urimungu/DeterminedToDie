using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public Powerups powerup;
    public AudioClip feedbackAudio;

    //detects collision and checks if it has a powerupcontroller
    public void OnTriggerEnter(Collider other)
    {
        PowerupController powerupController = other.GetComponent<PowerupController>();

        Debug.Log(powerupController);

        if (powerupController != null)
        {
            powerupController.AddPowerup(powerup);

            if (feedbackAudio != null)
                AudioSource.PlayClipAtPoint(feedbackAudio, transform.position, 1.0f);

            Destroy(this.gameObject);
        }
    }
}
