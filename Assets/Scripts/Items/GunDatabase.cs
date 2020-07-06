using System.Collections.Generic;
using UnityEngine;

public class GunDatabase : MonoBehaviour{

    public List<GunType> GunCatalog = new List<GunType>();

    //Initializes every weapon in the game
    private void Awake() {
        int id = 0;

        //Pistols
        GunCatalog.Add(new GunType("M1911", "It's a simple pistol", id, false, 12, 120, 0.3f, 1, 0.4f, 0.3f, 0.02f, 0.05f, 15, 1, 0, "GunIcons/Pistol"));

        //ShotGuns
        GunCatalog.Add(new GunType("Mossberg", "It's a ShotGun", ++id, false, 2, 60, 0.9f, 2, 1, 0.8f, 0.2f, 0.1f, 10, 5, 0, "GunIcons/ShotGun"));

        //Pulse Rifles
        GunCatalog.Add(new GunType("Pulse Rifle", "Shoots a burst of rounds", ++id, false, 8, 160, 0.6f, 2, 1, 0.8f, 0.04f, 0.05f, 10, 3, 0.05f, "GunIcons/AK"));

        //Auto Rifles
        GunCatalog.Add(new GunType("MP5", "Shoots a constant stream of bullets.", ++id, true, 30, 300, 0.4f, 2, 0.1f, 0.1f, 0.03f, 0.05f, 10, 1, 0, "GunIcons/AK"));
    }

}
