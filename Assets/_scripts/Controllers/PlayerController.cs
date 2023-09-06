using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField, Header("Skin Settings")] private GameObject _baseSkin;
    [SerializeField] private GameObject _ragdollSkin;
    [SerializeField] private Animator _animator;
    [SerializeField] private Rigidbody[] _ragdollRbs;
    [SerializeField] private float _kickForce = 50f;
    [SerializeField, Header("Move Settings")] private float _forwardMoveSpeed;
    [SerializeField] private float _sideMoveSpeed;
    [SerializeField] private float _moveAmplitude;
    [SerializeField, Header("Fx")] private ParticleSystem _wrapFx;
    private float _horizontalInput;
    private bool _canInput = false;
    private bool _canMove = false;
    private int _animKeyRun;
    private int _animKeyShoot;
    private Transform _transform;
    private IInput _input;

    private void OnEnable()
    {
        EventBus.OnGameStart += StartMove;
        EventBus.OnGameLose += Death;
    }
    private void OnDisable()
    {
        EventBus.OnGameStart -= StartMove;
        EventBus.OnGameLose -= Death;
    }

    private void Awake()
    {
        _input = new MouseTouchInput();
        _transform = transform;
    }

    private void Start()
    {
        AssignAnimKeys();
    }

    private void Update()
    {
        Input();
        Move();
        Shooting();
    }

    private void StartMove()
    {
        _canInput = true;
        _canMove = true;
        _wrapFx.Play();
    }

    private void Input()
    {
        if (!_canInput) return;

        _horizontalInput = _input.GetHorizontalInput();
    }

    private void Shooting()
    {
        if (!_canMove)
        {
            _animator.SetBool(_animKeyShoot, false);
            return;
        }

        _animator.SetBool(_animKeyShoot, true);
    }

    private void Move()
    {
        if (!_canMove)
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

        _canMove = false;
        _canInput = false;
    }

    private void AssignAnimKeys()
    {
        _animKeyRun = Animator.StringToHash("Run");
        _animKeyShoot = Animator.StringToHash("Shoot");
    }
}
