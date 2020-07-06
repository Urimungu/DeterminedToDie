using System;
using UnityEngine;

[Serializable]
public class GunType{
    
    //Stats
    public string Name;
    public string Description;
    public int ID;

    public bool AutomaticWeapon;

    public int ChamberSize;
    public int MaxAmmo;
    public float StowSpeed;

    public float ReloadTime;
    public float FireRate;
    public float GunRecoil;
    public float GunAccuracy;
    public float BulletTrailLifeTime;
    public float Damage;
    public float BulletsPerShot;
    public float TimePerShot;

    public string IconPath;
    public string ModelPath;    //the path that will be used for the gun

    /// <summary>
    /// Initializes the gun as an empty object.
    /// </summary>
    public GunType() { }

    /// <summary>
    /// Initializes the gun with all the values pre-determined.
    /// </summary>
    /// <param name="name">The name of the weapon.</param>
    /// <param name="desc">The description of the gun that will be displayed.</param>
    /// <param name="id">The ID or place in the Gun Database.</param>
    /// <param name="auto">If the will shoot rounds automatically.</param>
    /// <param name="chamber">The Amount of bullets that fit in the chamber. The bullets that can be fired before needing to reload.</param>
    /// <param name="stow">The amount of time it takes to draw the weapon</param>
    /// <param name="maxAmmo">The Maximum amount of ammo that this weapon can hold.</param>
    /// <param name="reload">The time it takes to reload this weapon.</param>
    /// <param name="rate">The amount of time between each bullet fired.</param>
    /// <param name="recoil">The amount of force the weapon will be shot upwards when fired.</param>
    /// <param name="acc">The Accuracy of the gun, or the spread of the gun.</param>
    /// <param name="trailLifeTime">The life time of the trail left behind by the gun.</param>
    /// <param name="damage">The amount of damage dealt per bullet.</param>
    /// <param name="bullets">The amount of bullets per shot. (Mostly used for shotguns or pulse rifles) </param>
    /// <param name="shotTime">The amount of time between each shot in the shot. (Mainly used for pulse rifles)</param>
    /// <param name="icon">The path in the resource folder for the gun display.</param>
    public GunType(string name, string desc, int id, bool auto, int chamber, int maxAmmo, float stow, float reload, float rate, float recoil, float acc, float trailLifeTime, float damage, float bullets, float shotTime, string icon) {
        Name = name;
        Description = desc;
        ID = id;

        AutomaticWeapon = auto;

        ChamberSize = chamber;
        MaxAmmo = maxAmmo;
        StowSpeed = stow;

        ReloadTime = reload;
        FireRate = rate;
        GunRecoil = recoil;
        GunAccuracy = acc;
        BulletTrailLifeTime = trailLifeTime;
        Damage = damage;
        BulletsPerShot = bullets;
        TimePerShot = shotTime;

        IconPath = icon;
    }
}
