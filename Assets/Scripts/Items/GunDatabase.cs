using System.Collections.Generic;
using UnityEngine;

public class GunDatabase : MonoBehaviour{

    public List<GunType> GunCatalog = new List<GunType>();

    //Initializes every weapon in the game
    private void Awake() {
        int id = 0;

        //Empty Gun
        GunCatalog.Add(new GunType());

        //Pistols
        GunCatalog.Add(new GunType("Pistol", "It's a simple pistol", ++id, false, 12, 120, 12, 120, 0.3f, 1, 0.4f, 0.3f, 0.02f, 0.01f, 15, 15, 1, 0.05f, "GunIcons/Pistol", "Guns/Pistol", ""));

        //ShotGuns
        GunCatalog.Add(new GunType("Mossberg", "It's a ShotGun", ++id, false, 2, 60, 2, 60, 0.9f, 2, 1, 0.8f, 0.05f, 0.1f, 15, 10, 5, 0, "GunIcons/ShotGun", "Guns/Mossberg", ""));

        //Pulse Rifles
        GunCatalog.Add(new GunType("Pulse Rifle", "Shoots a burst of rounds", ++id, false, 8, 160, 8, 160, 0.6f, 2, 1, 0.8f, 0.04f, 0.05f, 15, 10, 3, 0.05f, "GunIcons/AK", "", ""));

        //Auto Rifles
        GunCatalog.Add(new GunType("MP5", "Shoots a constant stream of bullets.", ++id, true, 30, 300, 30, 300, 0.4f, 2, 0.1f, 0.1f, 0.03f, 0.05f, 15, 10, 1, 0.05f, "GunIcons/AK", "Guns/MP5", ""));
    }

    /// <summary>
    /// Creates a new gun so it doesn't pass it by reference
    /// </summary>
    /// <param name="id">The index of the gun from the list.</param>
    /// <returns></returns>
    public GunType GetGun(int id) {
        var gun = new GunType();

        gun.Name = GunCatalog[id].Name;
        gun.Description = GunCatalog[id].Description;
        gun.ID = GunCatalog[id].ID;

        gun.AutomaticWeapon = GunCatalog[id].AutomaticWeapon;

        gun.ChamberSize = GunCatalog[id].ChamberSize;
        gun.MaxAmmo = GunCatalog[id].MaxAmmo;
        gun.ChamberAmmo = GunCatalog[id].ChamberAmmo;
        gun.TotalAmmo = GunCatalog[id].TotalAmmo;
        gun.StowSpeed = GunCatalog[id].StowSpeed;

        gun.ReloadTime = GunCatalog[id].ReloadTime;
        gun.FireRate = GunCatalog[id].FireRate;
        gun.GunRecoil = GunCatalog[id].GunRecoil;
        gun.GunAccuracy = GunCatalog[id].GunAccuracy;
        gun.BulletTrailLifeTime = GunCatalog[id].BulletTrailLifeTime;
        gun.Range = GunCatalog[id].Range;
        gun.Damage = GunCatalog[id].Damage;
        gun.BulletsPerShot = GunCatalog[id].BulletsPerShot;
        gun.TimePerShot = GunCatalog[id].TimePerShot;

        gun.IconPath = GunCatalog[id].IconPath;
        gun.ModelPath = GunCatalog[id].ModelPath;
        gun.CrossHair = GunCatalog[id].CrossHair;

        return gun;
    }

}
