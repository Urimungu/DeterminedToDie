using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;

public class CharacterFunctions : CharacterStats {

    //Starts up the player
    protected void InitializePlayer() {
        CameraController.FollowPlayer(0,0,this);
        CanLook = CanLook;
    }

    //Vitality
    public float CurrentHealth {
        get => _currentHealth;
        set {
            //Checks to see if play is still alive
            if (value <= 0) 
                Death();
            

            _currentHealth = value;
        }
    }
    public float MaxHealth {
        get => _maxHealth;
        set => _maxHealth = value;
    }

    //Stats
    public float CrouchingSpeed {
        get => _crouchingSpeed;
        set => _crouchingSpeed = value;
    }
    public float WalkingSpeed {
        get => _walkingSpeed;
        set => _walkingSpeed = value;
    }
    public float RunningSpeed {
        get => _runningSpeed;
        set => _runningSpeed = value;
    }
    public float AimingWalkSpeed {
        get => _aimingWalkSpeed;
        set => _aimingWalkSpeed = value;
    }
    public float AirSpeed {
        get => _airSpeed;
        set => _airSpeed = value;
    }
    public float JumpForce {
        get => _jumpForce;
        set => _jumpForce = value;
    }
    public MovementState MoveState {
        get => _moveState;
        set {
            switch (value) {
                case MovementState.Aiming:
                    CanShoot = true;
                    CurrentMovementSpeed = AimingWalkSpeed;
                    break;
                case MovementState.Crouching:
                    CanShoot = true;
                    CurrentMovementSpeed = CrouchingSpeed;
                    break;
                case MovementState.Running:
                    CanShoot = false;
                    CurrentMovementSpeed = RunningSpeed;
                    break;
                case MovementState.Walking:
                    CanShoot = true;
                    CurrentMovementSpeed = WalkingSpeed;
                    break;
            }

            _moveState = value;
        }
    }

    //Variables
    public bool CanRecieveInput {
        get => _canRecieveInput;
        set {
            _canRecieveInput = value;
        }
    }
    public bool CanMove {
        get => _canMove;
        set => _canMove = value;
    }
    public float CheckGroundRay {
        get => _checkGroundRay;
        set => _checkGroundRay = value;
    }
    public LayerMask GroundMask {
        get => _groundMask;
        set => _groundMask = value;
    }

    //Camera Options
    public bool CanLook {
        get => _canLook;
        set {
            //Controls if the Mouse is locked or not
            if (value){
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }else{
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }

            _canLook = value;

        }
    }
    public float HorizontalSensitivity {
        get => _horizontalSensitivity;
        set => _horizontalSensitivity = value;
    }
    public float VerticalSensitivity {
        get => _verticalSensitivity;
        set => _verticalSensitivity = value;
    }
    public float CameraHeight {
        get => _cameraHeight;
        set => _cameraHeight = value;
    }
    public float CameraDistance {
        get => _cameraDistance;
        set => _cameraDistance = value;
    }
    public float CameraMinDistance {
        get => _cameraMinDistance;
        set => _cameraMinDistance = value;
    }
    public float CameraHorizontalOffset {
        get => _cameraHorizontalOffset;
        set => _cameraHorizontalOffset = value;
    }
    public float CameraFOV {
        get => _cameraFOV;
        set => _cameraFOV = value;
    }
    public bool HoverRight {
        get => _hoverRight;
        set => _hoverRight = value;
    }
    public LayerMask CameraMask {
        get => _cameraMask;
        set => _cameraMask = value;
    }

    //Running
    public float RunCameraDistance {
        get => _runCameraDistance;
        set => _runCameraDistance = value;
    }
    public float RunCamHorizontalOffset {
        get => _runCamHorizontalOffset;
        set => _runCamHorizontalOffset = value;
    }
    public float RunningFOV {
        get => _runningFOV;
        set => _runningFOV = value;
    }

    //Aiming
    public float HorizontalAimSensitivity {
        get => _horizontalAimSensitivity;
        set => _horizontalAimSensitivity = value;
    }
    public float VerticalAimSensitivity {
        get => _verticalAimSensitivity;
        set => _verticalAimSensitivity = value;
    }
    public float AimCameraDistance {
        get => _aimCameraDistance;
        set => _aimCameraDistance = value;
    }
    public float AimingFOV {
        get => _aimingFOV;
        set => _aimingFOV = value;
    }
    public LayerMask ShootMask {
        get => _shootMask;
        set => _shootMask = value;
    }

    //Gun
    public bool CanShoot {
        get => _canShoot;
        set => _canShoot = value;
    }
    public GunType Primary {
        get => _primary;
        set {
            //Clears the Old Gun First
            if (WeaponHandle.childCount != 0) Destroy(WeaponHandle.GetChild(0).gameObject);

            _primary = value;

            //Exits if there isn't a gun to create
            if (value.ID == 0) return;
            

            //Spawns in the new weapon model
            var mesh = Resources.Load<GameObject>(value.ModelPath);
            if (mesh == null) return;

            //Creates the gun
            var model = Instantiate(mesh, WeaponHandle);
            model.transform.localPosition = new Vector3(0, 0, 0);
            model.transform.localRotation = Quaternion.identity;
            model.name = "GunHolder";
        }
    }
    public GunType Secondary {
        get => _secondary;
        set => _secondary = value;
    }
    public Transform WeaponHandle {
        get => _weaponHandle != null ? _weaponHandle : _weaponHandle = transform.Find("Gun/MeshHolder");
    }

    //Dynamic Variables
    public float CurrentMovementSpeed {
        get => _currentMovementSpeed;
        set => _currentMovementSpeed = value;
    }
    public float RadiusStep {
        get => _radiusStep;
        set => _radiusStep = value;
    }
    public float HorOffsetStep {
        get => _horOffsetStep;
        set => _horOffsetStep = value;
    }
    public float FOVStep {
        get => _fovStep;
        set => _fovStep = value;
    }
    public float SpeedStep {
        get => _speedStep;
        set => _speedStep = value;
    }
    public float CurrentCameraRadius {
        get => _currentCameraRadius;
        set => _currentCameraRadius = value;
    }
    public float CurrentHorizontalOffset {
        get => _currentHorizontalOffset;
        set => _currentHorizontalOffset = value;
    }
    public float CurrentFOV {
        get => _currentFOV;
        set => _currentFOV = value;
    }
    public bool Grounded {
        get => _grounded;
        set => _grounded = value;
    }

    //References
    public Rigidbody PlayerRigidbody
    {
        get => _rigidbody != null ? _rigidbody : _rigidbody = GetComponent<Rigidbody>();
    }
    public GameObject PlayerCamera{
        get => _playerCamera != null ? _playerCamera : (_playerCamera = CreateCamera());
    }
    public GameObject Corpse
    {
        get
        {
            if (_corpse == null)
            {
                var body = Instantiate(Resources.Load<GameObject>("Prefabs/Ragdoll/DookieRagdoll"), transform.position, transform.rotation);
                return _corpse = body;
            }

            return _corpse;
        }
    }
    public Animator Anim {
        get => _anim != null ? _anim : _anim = transform.Find("Mesh").GetComponent<Animator>();
    }
    public Transform ShootPoint
    {
        get => _shootPoint != null ? _shootPoint : _shootPoint = transform.Find("Gun/MeshHolder/GunHolder/ShootPoint");
    }
    public Transform GunHolder{
        get => _gunHolder != null ? _gunHolder : (_gunHolder = transform.Find("Gun"));
    }

    //Additional Variables
    protected float _shootTimer;
    protected bool _isReloading;
    protected bool _isShooting;
    public bool IsDead {
        get => _isDead;
        set => _isDead = value;
    }

    //Additional Functions
    private GameObject CreateCamera()
    {
        //Creates Game Objects
        GameObject camHolder = new GameObject("CameraHolder");
        GameObject cam = new GameObject("Camera");

        //Creates the Camera Components
        cam.AddComponent<Camera>();
        cam.AddComponent<AudioListener>();

        //Sets reference parent and sets to zero
        cam.transform.parent = camHolder.transform;
        cam.transform.position = new Vector3(0, 0, -2);
        cam.tag = "MainCamera";

        return camHolder;
    }
    public bool CheckIfGrounded{
        get{
            var colliderBase = transform.position - new Vector3(0, (GetComponent<CapsuleCollider>().height / 2) - 0.3f, 0);
            Ray ray = new Ray(colliderBase, Vector3.down);
            Grounded = Physics.SphereCast(ray, 0.25f, CheckGroundRay, GroundMask);

            Anim.SetBool("Grounded", Grounded);

            return Grounded;
        }
    }
    public void TakeDamage(float amount){
        CurrentHealth -= amount;

        //Updates the HUD if it isn't missing
        if (GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdateHealth(CurrentHealth, MaxHealth);
    }

    //Weapon Options
    public void PickUpGun(GunType gun) {
        //Swaps guns if there is room in the secondary hand
        if (Primary.ID != 0 && Secondary.ID == 0)
            SwapWeapons();
        else if (Primary.ID != 0 && Secondary.ID != 0)
            DropGun(false);
        
        //Updates the guns and the display
        Primary = gun;
        UpdateGunDisplay();
    }
    public void DropGun(bool swap = true) {
        //Exits if the player has no weapon equipped
        if (Primary.ID == 0) return;

        var gun = Instantiate(Resources.Load<GameObject>("PickUps/Weapons/" + Primary.Name)).transform;

        //Sets the transforms
        gun.position = WeaponHandle.position;
        gun.rotation = WeaponHandle.rotation;

        //Adds a little spin to it
        gun.GetComponent<Rigidbody>().AddForce(gun.up + (gun.right * 0.3f) * 20, ForceMode.Impulse);
        gun.GetComponent<Rigidbody>().AddTorque((gun.up * 0.3f) + gun.right * 30, ForceMode.Impulse);
        gun.GetComponent<GunPickUp>().UpdateAmmo(Primary);

        //Clears the weapon
        var newGun = new GunType();
        Primary = newGun;
        
        //Swaps the secondary in the Primary place if they have a secondary
        if (Secondary.ID != 0 && swap)
            SwapWeapons();

        //Updates the Display
        UpdateGunDisplay();

    }
    public void Death() {
        //Kills the player
        CanRecieveInput = false;
        IsDead = true;

        //Disables Colliders and Rigidbody
        transform.GetComponent<Rigidbody>().isKinematic = true;
        transform.GetComponent<CapsuleCollider>().enabled = false;
        WeaponHandle.gameObject.SetActive(false);
        transform.Find("Mesh").gameObject.SetActive(false);

        //Starts Over
        StartCoroutine(Respawn(5));
    }

    //Protected Functions
    protected void GunController(){
        //Position of the start of the radius
        var lookPosition = PlayerCamera.transform.GetChild(0).position + (PlayerCamera.transform.GetChild(0).forward * CameraDistance);
        var lookPoint = lookPosition + (PlayerCamera.transform.GetChild(0).forward * 10);

        //Raycasts forward to see if it hit anything
        if (Physics.Raycast(lookPosition, PlayerCamera.transform.GetChild(0).forward, out RaycastHit hit, 500, ShootMask)){
            if (hit.collider != null){
                //Updates the display
                if (hit.collider.GetComponent<Interactable>() != null && hit.collider.GetComponent<Interactable>().InteractDistance() > hit.distance){
                    UpdateInputDisplay(hit.collider.GetComponent<Interactable>().GetMessage());

                    //Presses E to interact
                    if (Input.GetKeyDown(KeyCode.E))
                        hit.collider.GetComponent<Interactable>().Interact(this);
                } else
                    UpdateInputDisplay();

                lookPoint = hit.point;
            }
        }

        //Looks at the camera aiming position
        GunHolder.LookAt(lookPoint);
    }
    protected void UpdateGunDisplay() {
        if (GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdateWeapons(Primary, Secondary);
    }
    
    protected void UpdateInputDisplay(string message = "") {
        if (GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdateInputDisplay(message);
    }
    protected void UpdatePrimary(){
        if (GameManager.Instance != null && GameManager.Instance.HUD != null)
            GameManager.Instance.HUD.UpdatePrimaryAmmo(Primary.ChamberAmmo, Primary.TotalAmmo);
        
    }
    protected void SwapWeapons() {
        _shootTimer = Time.time + Secondary.StowSpeed;
        //Switches gun
        var tempGun = Primary;
        Primary = Secondary;
        Secondary = tempGun;
    }

    //Timed Functions
    protected IEnumerator ReloadGun(){
        _isReloading = true;

        //Waits for the reload time
        yield return new WaitForSeconds(Primary.ReloadTime);
        var missingAmmo = Mathf.Clamp(Primary.ChamberSize - Primary.ChamberAmmo, 0, Primary.TotalAmmo);
        Primary.ChamberAmmo += missingAmmo;
        Primary.TotalAmmo -= missingAmmo;

        //Updates the HUD if it isn't missing
        UpdatePrimary();
        _isReloading = false;
    }
    protected IEnumerator ShootGun(){
        _isShooting = true;

        //Applies the Recoil for the gun
        CameraController.RecoilCamera(Primary.GunRecoil, this);
        _shootTimer = Time.time + Primary.FireRate;
        Primary.ChamberAmmo--;

        for (int i = 0; i < Primary.BulletsPerShot; i++){
            //Creates the asthetics of damage being taken
            var accuracy = new Vector3(Random.Range(-Primary.GunAccuracy, Primary.GunAccuracy), Random.Range(-Primary.GunAccuracy, Primary.GunAccuracy), 0);
            accuracy = MoveState == MovementState.Aiming ? accuracy / 3 : accuracy;
            var newForward = GunHolder.forward + accuracy;
            var sparks = "FX/Sparks";

            //Applies the Shots
            Physics.Raycast(ShootPoint.position, newForward, out RaycastHit hit, Primary.Range, ShootMask);

            if (hit.collider != null){
                //Hits an enemy and deals damage
                if (hit.collider.GetComponent<EnemyFunctions>() != null){
                    //Damage that that the player will deal
                    var damage = Primary.Damage * (1 - (hit.distance / Primary.Range));
                    hit.collider.GetComponent<EnemyFunctions>().TakeDamage(damage, transform);
                    sparks = "FX/Blood";

                    //Creates the text
                    StartCoroutine(CreateTextMarker(damage, hit.point));
                }

                //Creates Sparks
                StartCoroutine(CreateSparks(0.25f, hit.point, hit.normal, sparks));
            }

            //Creates the Muzzle Flash
            ShootPoint.gameObject.SetActive(true);
            yield return new WaitForSeconds(Primary.TimePerShot);
            ShootPoint.gameObject.SetActive(false);

            //Removes the markers after they were created
            var hitDistance = hit.distance != 0 ? hit.distance : Primary.Range;
            StartCoroutine(CreateBulletTrial(Primary.BulletTrailLifeTime, hitDistance, newForward));
        }
        _isShooting = false;
    }
    protected IEnumerator CreateSparks(float timer, Vector3 spawnPosition, Vector3 rotation, string sparksPath){
        //Initializes value
        var effects = Instantiate(Resources.Load<GameObject>(sparksPath)).transform;

        //Sets the transforms
        effects.position = spawnPosition;
        effects.rotation = Quaternion.FromToRotation(Vector3.forward, rotation);

        ////Destroys itself after it's life-time has expired
        yield return new WaitForSeconds(timer);
        Destroy(effects.gameObject);
    }
    protected IEnumerator CreateBulletTrial(float timer, float hitDistance, Vector3 direction) {
        //Initializes the values
        var trail = Instantiate(Resources.Load<GameObject>("FX/BulletTrail")).transform;

        //Sets the Gun Trail
        trail.position = ShootPoint.position + (direction * (hitDistance / 2));
        trail.LookAt(ShootPoint);
        trail.localScale = new Vector3(1, 1, (hitDistance * 4));

        //Destroys the object after a while
        yield return new WaitForSeconds(timer);
        Destroy(trail.gameObject);
    }
    protected IEnumerator CreateTextMarker(float damage, Vector3 spawnPoint){
        //Initializes values
        var damageText = Instantiate(Resources.Load<GameObject>("FX/DamageMarker")).transform;
        var color = Color.white;
        var mesh = damageText.GetChild(0).GetComponent<TextMesh>();

        //Sets the transforms
        damageText.position = spawnPoint;
        damageText.LookAt(transform.position);
        mesh.text = damage.ToString("00");
        mesh.color = color;

        //Moves the text up until it is destroyed
        for (int i = 0; i < 20; i++){
            yield return new WaitForSeconds(0.01f);
            damageText.position += new Vector3(0, 0.1f, 0);
            color.a -= 0.05f;
            mesh.color = color;
        }

        Destroy(damageText.gameObject);
    }
    protected IEnumerator Respawn(float time) {
        yield return new WaitForSeconds(time);
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }
}
