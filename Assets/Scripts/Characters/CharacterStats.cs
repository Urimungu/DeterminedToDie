using UnityEngine;

public class CharacterStats : MonoBehaviour{

    [Header("Vitality")]
    [SerializeField] protected float _currentHealth = 100;
    [SerializeField] protected float _maxHealth = 100;

    [Header("Stats")]
    [SerializeField] protected float _crouchingSpeed = 4;
    [SerializeField] protected float _walkingSpeed = 10;
    [SerializeField] protected float _runningSpeed = 15;
    [SerializeField] protected float _aimingWalkSpeed = 6;
    [SerializeField] protected float _airSpeed = 3;
    [SerializeField] protected float _jumpForce = 3;
    [SerializeField] protected MovementState _moveState;

    [Header("Variables")]
    [SerializeField] protected bool _canRecieveInput = true;
    [SerializeField] protected bool _canMove = true;
    [SerializeField] protected float _checkGroundRay = 0.3f;
    [SerializeField] protected LayerMask _groundMask;

    [Header("Camera Options")]
    [SerializeField] protected bool _canLook = true;
    [SerializeField] protected float _horizontalSensitivity = 3;
    [SerializeField] protected float _verticalSensitivity = 0.2f;
    [SerializeField] protected float _cameraHeight = 1;
    [SerializeField] protected float _cameraDistance = 2.5f;
    [SerializeField] protected float _cameraMinDistance = 0.2f;
    [SerializeField] protected float _cameraHorizontalOffset = 1;
    [SerializeField] protected float _cameraFOV = 60;
    [SerializeField] protected bool _hoverRight = true;
    [SerializeField] protected LayerMask _cameraMask;

    [Header("Running")]
    [SerializeField] protected float _runCameraDistance = 3.25f;
    [SerializeField] protected float _runCamHorizontalOffset = 0;
    [SerializeField] protected float _runningFOV = 80;

    [Header("Aiming")]
    [SerializeField] protected float _horizontalAimSensitivity = 1.5f;
    [SerializeField] protected float _verticalAimSensitivity = 0.05f;
    [SerializeField] protected float _aimCameraDistance = 1.5f;
    [SerializeField] protected float _aimingFOV = 40;
    [SerializeField] protected LayerMask _shootMask;

    [Header("Gun")]
    [SerializeField] protected bool _canShoot = true;
    [SerializeField] protected GunType _primary;
    [SerializeField] protected GunType _secondary;
    [SerializeField] protected Transform _weaponHandle;

    [Header("Dynamic Variables")]
    [SerializeField] protected float _currentMovementSpeed = 10;
    [SerializeField] protected float _radiusStep = 0.7f;
    [SerializeField] protected float _horOffsetStep = 0.3f;
    [SerializeField] protected float _fovStep = 0.7f;
    [SerializeField] protected float _speedStep = 0.5f;
    [SerializeField] protected bool _grounded = true;
    [SerializeField] protected bool _isDead = false;

    [HideInInspector] protected float _currentCameraRadius;
    [HideInInspector] protected float _currentHorizontalOffset;
    [HideInInspector] protected float _currentFOV;

    [Header("References")]
    [SerializeField] protected Rigidbody _rigidbody;
    [SerializeField] protected GameObject _playerCamera;
    [SerializeField] protected GameObject _corpse;
    [SerializeField] protected Animator _anim;
    [SerializeField] protected Transform _gunHolder;
    [SerializeField] protected Transform _shootPoint;


    //Additional Values
    public enum MovementState { Walking, Running, Crouching, Aiming };
}
