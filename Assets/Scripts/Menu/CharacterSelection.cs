using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelection : MonoBehaviour
{
    private int numOfPlayers = 0;
    private int playerSelect = 1;

    private enum Character
    {
        Dookie,
        Scat,
        DoubleDoo,
        Slops
    }
    private Character character;

    private void Update()
    {
        if (playerSelect == numOfPlayers)
            playerSelect = 5;
        PlayerStateMachine();
    }

    void PlayerStateMachine()
    {
        switch (playerSelect)
        {
            case 1:
                PlayerOneSelect();
                break;
            case 2:
                PlayerTwoSelect();
                break;
            case 3:
                PlayerThreeSelect();
                break;
            case 4:
                PlayerFourSelect();
                break;
            case 5:
                AllSelected();
                break;
            default:
                Debug.Log("Not a selected player");
                break;
        }
    }

    void PlayerOneSelect()
    {

    }

    void PlayerTwoSelect()
    {

    }

    void PlayerThreeSelect()
    {

    }

    void PlayerFourSelect()
    {

    }

    void AllSelected()
    {

    }
}
