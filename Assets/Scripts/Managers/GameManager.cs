using UnityEngine;

[RequireComponent(typeof (GunDatabase))]
public class GameManager : Singleton<GameManager>{

    [Header("References")]
    public CharacterStats Player;
    public HUDManager HUD;
    public GunDatabase WeaponDatabase { get { return GetComponent<GunDatabase>(); } }

}
