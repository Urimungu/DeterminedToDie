using UnityEngine.UI;
using UnityEngine;

public class HUDManager : MonoBehaviour {

    [Header("CrossHair")]
    [SerializeField] protected Image _crossHair;

    [Header("Health")]
    [SerializeField] protected GameObject _healthHolder;
    [SerializeField] protected Color _healthFillColor;
    [SerializeField] protected Image _healthFillRight;
    [SerializeField] protected Image _healthFillLeft;
    [SerializeField] protected Image _healthBackground;
    [SerializeField] protected Text _healthText;

    [Header("Primary")]
    [SerializeField] protected GameObject _primaryHolder;
    [SerializeField] protected Image _primaryGunIcon;
    [SerializeField] protected Text _primaryAmmoCount;

    [Header("Secondary")]
    [SerializeField] protected GameObject _secondaryHolder;
    [SerializeField] protected Image _secondaryGunIcon;
    [SerializeField] protected Text _secondaryAmmoCount;

    [Header("Objectives")]
    [SerializeField] protected GameObject _objectiveDisplay;
    [SerializeField] protected Text _objectiveText;
    [SerializeField] protected GameObject _objectiveMarker;
    [SerializeField] protected Text _objectiveMarkerText;

    [Header("InputDispaly")]
    [SerializeField] protected GameObject _inputDisplay;
    [SerializeField] protected Text _inputText;

}
