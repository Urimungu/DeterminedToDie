using UnityEngine;

[RequireComponent(typeof (GunDatabase))]
public class GameManager : Singleton<GameManager>{

    [Header("References")]
    public CharacterFunctions Player;
    public HUDController HUD;
    public GunDatabase WeaponDatabase { get { return GetComponent<GunDatabase>(); } }
    public LevelFunctions CurrentLevel;

    [Header("Stats")]
    public int ZombiesKilled = 0;

}
