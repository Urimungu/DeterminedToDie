using UnityEngine;

public class Interactable : MonoBehaviour{

    [Header("Stats")]
    [SerializeField] protected bool _interactable;
    [TextArea()]
    [SerializeField] protected string _displayMessage;
    [SerializeField] protected float _interactDistance;

    //Interaction
    protected delegate void Interaction(CharacterFunctions player);

    protected Interaction _interactDeclare;
    protected Interaction InteractDeclare {
        get => _interactDeclare;
        set => _interactDeclare = value;
    }

    //Called Functions
    public void Interact(CharacterFunctions player) {
        InteractDeclare(player);
    }
    public string GetMessage() {
        return _displayMessage;
    }
    public float InteractDistance() {
        return _interactDistance;
    }
}
