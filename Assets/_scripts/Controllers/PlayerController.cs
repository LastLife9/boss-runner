using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("Skin Settings")] private GameObject _baseSkin;
    [SerializeField] private GameObject _ragdollSkin;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody[] _ragdollRbs;
    [SerializeField] private float _kickForce = 50f;
    [SerializeField, Header("Move Settings")] private float _forwardMoveSpeed;
    [SerializeField] private float _sideMoveSpeed;
    [SerializeField] private float _moveAmplitude;
    [SerializeField, Header("Fx")] private ParticleSystem _wrapFx;
    private const string _bulletTag = "Bullet";
    private float _horizontalInput;
    private float _shootingSpeed;
    private bool _canInput = false;
    private bool _move = false;
    private bool _shoot = false;
    private int _animKeyRun;
    private int _animKeyShoot;
    private Transform _transform;
    private Transform _target;
    private IInput _input;

    private void OnEnable()
    {
        EventBus.OnGameStart += StartMove;
        EventBus.OnGameLose += Death;
        EventBus.OnBossFightStart += StopMove;
    }
    private void OnDisable()
    {
        EventBus.OnGameStart -= StartMove;
        EventBus.OnGameLose -= Death;
        EventBus.OnBossFightStart -= StopMove;
    }

    private void Awake()
    {
        _input = new MouseTouchInput();
        _transform = transform;
    }

    private void Start()
    {
        GetShootingParams();
        AssignAnimKeys();
    }

    private void Update()
    {
        Input();
        Move();
        Shooting();
        if (_target != null) RotateToTarget(_target);
    }

    private void StartMove()
    {
        _canInput = true;
        _move = true;
        _shoot = true;
        _wrapFx.Play();
    }

    private void StopMove()
    {
        _canInput = false;
        _move = false;
        _shoot = true;
        _target = FindObjectOfType<BossEnemy>().transform;
        _wrapFx.Stop();
    }

    private void Input()
    {
        if (!_canInput) return;

        _horizontalInput = _input.GetHorizontalInput();
    }

    private void Shooting()
    {
        if (!_shoot)
        {
            _animator.SetBool(_animKeyShoot, false);
            return;
        }

        _shootingSpeed -= Time.deltaTime;
        if(_shootingSpeed <= 0)
        {
            GetShootingParams();
            ObjectPooling.Instance.SpawnFromPool(_bulletTag, _firePoint.position, _firePoint.rotation);
        }

        _animator.SetBool(_animKeyShoot, true);
    }

    private void Move()
    {
        if (!_move)
        {
            _animator.SetBool(_animKeyRun, false);
            return;
        }

        float _sideMoveDelta = _horizontalInput * _sideMoveSpeed * Time.deltaTime;
        float _forwardMove = _forwardMoveSpeed * Time.deltaTime;

        _transform.Translate(new Vector3(_sideMoveDelta, 0f, _forwardMove));

        _transform.position = new Vector3(
            Mathf.Clamp(_transform.position.x, -_moveAmplitude, _moveAmplitude),
            _transform.position.y,
            _transform.position.z);

        _animator.SetBool(_animKeyRun, true);
    }

    private void RotateToTarget(Transform target)
    {
        Vector3 direction = (target.position - _transform.position).normalized;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            _transform.rotation = Quaternion.Slerp(_transform.rotation, lookRotation, Time.deltaTime * _forwardMoveSpeed);
        }
    }

    private void Death()
    {
        _baseSkin.SetActive(false);
        _ragdollSkin.SetActive(true);
        _wrapFx.Stop();

        foreach (Rigidbody rb in _ragdollRbs)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(Vector3.forward * _kickForce, ForceMode.VelocityChange);
        }

        _shoot = false;
        _move = false;
        _canInput = false;
        GetComponent<Collider>().enabled = false;
    }

    private void AssignAnimKeys()
    {
        _animKeyRun = Animator.StringToHash("Run");
        _animKeyShoot = Animator.StringToHash("Shoot");
    }

    private void GetShootingParams()
    {
        _shootingSpeed = GameConfig.Instance.GameParameters.shootingSpeed;
    }
}
