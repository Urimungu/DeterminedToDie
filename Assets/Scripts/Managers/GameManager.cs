using UnityEngine;

[RequireComponent(typeof (GunDatabase))]
public class GameManager : Singleton<GameManager>{

    [Header("References")]
    public CharacterFunctions Player;
    public HUDController HUD;
    public GunDatabase WeaponDatabase { get { return GetComponent<GunDatabase>(); } }
    public LevelFunctions CurrentLevel;

    //Public properties
    private Transform _zombieHolder;
    public Transform ZombieHolder {
        get => _zombieHolder != null ? _zombieHolder : _zombieHolder = new GameObject("ZombieHolder").transform;
    }

    [Header("Stats")]
    public int ZombiesKilled = 0;

}
