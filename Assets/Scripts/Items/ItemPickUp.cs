using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable{

    //Initializes itself as a pickup
    private void Start(){
        InteractDeclare = PickUp;
    }

    //is Destroyed if the player picks it up
    private void PickUp(CharacterFunctions player) {
        Destroy(gameObject);
    }

}
