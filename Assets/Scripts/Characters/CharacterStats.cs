using UnityEditor;
using UnityEngine;

public class CharacterStats : MonoBehaviour{

    [Header("Vitality")]
    public float CurrentHealth = 100;
    public float MaxHealth = 100;

    [Header("Stats")]
    public float CrouchingSpeed = 4;
    public float WalkingSpeed = 10;
    public float RunningSpeed = 15;
    public float AimingWalkSpeed = 6;
    public float AirSpeed = 3;
    public float JumpForce = 3;
    public enum MovementState { Walking, Running, Crouching, Aiming};
    public MovementState MoveState;

    [Header("Variables")]
    public bool CanMove = true;
    public float CheckGroundRay = 0.3f;
    public LayerMask GroundMask;

    [Header("Camera Options")]
    public float HorizontalSensitivity = 3;
    public float VerticalSensitivity = 0.2f;
    public float CameraHeight = 1;
    public float CameraDistance = 2.5f;
    public float CameraMinDistance = 0.2f;
    public float CameraHorizontalOffset = 1;
    public float CameraFOV = 60;
    public bool HoverRight = true;
    public LayerMask CameraMask;

    [Header("Running")]
    public float RunCameraDistance = 3.25f;
    public float RunCamHorizontalOffset = 0;
    public float RunningFOV = 80;

    [Header("Aiming")]
    public float HorizontalAimSensitivity = 1.5f;
    public float VerticalAimSensitivity = 0.05f;
    public float AimCameraDistance = 1.5f;
    public float AimingFOV = 40;
    public LayerMask ShootMask;

    [Header("Gun")]
    public bool AutomaticWeapon = false;
    public int CurrentChamberAmmo = 6;
    public int ChamberSize = 6;
    public int CurrentAmmo = 240;
    public int MaxAmmo = 240;
    public float ReloadTime = 2;
    public float FireRate = 0.4f;
    public float GunRecoil = 1f;
    public float GunAccuracy = 0.3f;
    public float BulletTrailLifeTime = 0.05f;
    public float Damage = 30;

    [Header("Current Stat Transition Speed")]
    //How fast the transitions are
    public float RadiusStep = 0.7f;
    public float HorOffsetStep = 0.3f;
    public float FOVStep = 0.7f;
    public float SpeedStep = 0.5f;

    [Header("Private References (Auto-Filled)")]
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private GameObject _playerCamera;
    [SerializeField] private Transform _gunHolder;
    [SerializeField] private Transform _shootPoint;

    //Variables that don't need to be touched in the Inspector
    [HideInInspector] public float CurrentCameraRadius;
    [HideInInspector] public float CurrentHorizontalOffset;
    [HideInInspector] public float CurrentFOV;

    #region Properties
    public bool CheckIfGrounded {
        get {
            var colliderBase = transform.position - new Vector3(0, (GetComponent<CapsuleCollider>().height / 2) - 0.3f, 0);
            Ray ray = new Ray(colliderBase, Vector3.down);
            return Physics.SphereCast(ray, 0.25f, CheckGroundRay, GroundMask);
        }
    }

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

    public Transform ShootPoint {
        get {
            if(_shootPoint == null)
                _shootPoint = transform.Find("Gun/ShootPoint");
            return _shootPoint;
        }
    }

    public Transform GunHolder {
        get {
            if(_gunHolder == null)
                _gunHolder = transform.Find("Gun");
            return _gunHolder;
        }
    }
    #endregion
}
