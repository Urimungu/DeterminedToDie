using UnityEngine;

[System.Serializable]
public class ObjectiveStruct{

    public string Objective;
    public enum ObjectiveType { Nothing, PickUp, Destination, Wait, Kill, ExitGame}
    public enum ObjectActionType { Nothing, Start, Exit}

    //Remove Return Items
    public ObjectActionType RemoveObject, ReturnObject;
    public GameObject Remove;
    public GameObject Return;

    //Pick Ups
    public Transform PickUp;

    //Get to the Destination
    public Transform Destination;
    public float DestinationDistance;

    //Waiting
    public float WaitTime;

    //Kill Enemies
    public int EnemiesToKill;

    //Exit Game
    public string LeveltoLoad;
    public float ExitTime;

    //Creates Empty Struct
    public ObjectiveStruct() { }

}
