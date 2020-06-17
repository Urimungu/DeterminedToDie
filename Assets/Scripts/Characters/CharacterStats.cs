using UnityEngine;

public class CharacterStats : MonoBehaviour{

    [Header("Stats")]
    public float CrouchingSpeed = 5;
    public float WalkingSpeed = 10;
    public float RunningSpeed = 15;

    [Header("Variables")]
    public bool CanMove = true;

    [Header("Camera Options")]
    public float HorizontalSensitivity = 5;
    public float VerticalSensitivity = 3;
    public float CameraHeight = 10;
    public float CameraDistance = 10;
    public float CameraMinDistance = 1;
    public float CameraHorizontalOffset = 10;
    public bool HoverRight = true;
    public LayerMask CameraMask;

    [Header("Private References (Auto-Filled)")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _playerCamera;


    #region Properties
    public Rigidbody RigidBody {
        get {
            if(_rigidbody == null)
                _rigidbody = GetComponent<Rigidbody>();
            return _rigidbody;
        }
    }

    public GameObject PlayerCamera {
        get {
            //Builds the Third-Person Camera Controller if there isn't one
            if(_playerCamera == null) {
                //Creates Game Objects
                GameObject camHolder = new GameObject("CameraHolder");
                GameObject cam = new GameObject("Camera");

                //Creates the Camera Components
                cam.AddComponent<Camera>();
                cam.AddComponent<AudioListener>();

                //Sets reference parent and sets to zero
                cam.transform.parent = camHolder.transform;
                cam.transform.position = new Vector3(0, 0, -2);

                //Sets the reference
                _playerCamera = camHolder;
            }
            return _playerCamera;
        }
    }
    #endregion
}
