using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour {
    [Header("Variables")]
    [SerializeField] private int numOfPlayers = 0;
    [SerializeField] private Button _playerOne;

    [Header("References")]
    [SerializeField] private Button _startButton;
    public Button StartButton { get => _startButton ?? (_startButton = transform.Find("Buttons/Start").GetComponent<Button>()); }
    private void Start() {
        StartButton.gameObject.SetActive(false);
    }

    //Chooses what character the player selected
    public void CharacterSelect(GameObject button) {
        var playerText = button.transform.Find("ChosenPlayer").GetComponent<Text>();

        //Selectes the currently selected character
        if(playerText.text == "")
            ChooseCharacter(button);
        else
            RemoveCharacter();
    }

    //Selects the now chosen character
    private void ChooseCharacter(GameObject button) {
        //Removes previously selected
        if(_playerOne != null) RemoveCharacter();

        //Initializes variables
        _startButton.gameObject.SetActive(true);
        var playerText = button.transform.Find("ChosenPlayer").GetComponent<Text>();
        var path = "Images/" + button.name + "TitleCard";
        button.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        playerText.text = "PLAYER ONE";

        //Unlinks the previous player if it had one
        if(_playerOne != null)
            _playerOne.transform.Find("ChosenPlayer").GetComponent<Text>().text = "";

        //Sets the new Player
        _playerOne = button.GetComponent<Button>();
    }

    //Unselectes the currently selected character
    private void RemoveCharacter() {
        //Initializes
        var path = "Images/" + _playerOne.gameObject.name + "TitleCardUnSelected";
        _startButton.gameObject.SetActive(false);

        //Removes the signs
        _playerOne.transform.Find("ChosenPlayer").GetComponent<Text>().text = "";
        _playerOne.gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(path);
        _playerOne = null;
    }
}
