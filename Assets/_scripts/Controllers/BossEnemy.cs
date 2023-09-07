using UnityEngine;

public class BossEnemy : EnemyBase, IDamagable
{
    private int _moveAnimKey = Animator.StringToHash("Move");
    private float _bossSpeed;
    private bool _move;
    private Transform _targetT;
    [SerializeField] private Animator _animator;

    private void OnEnable()
    {
        EventBus.OnBossFightStart += () => { _move = true; };
    }

    private void OnDisable()
    {
        EventBus.OnBossFightStart -= () => { _move = true; };
    }

    private void Start()
    {
        _health = GameConfig.Instance.GameParameters.bossHealth;
        _bossSpeed = GameConfig.Instance.GameParameters.bossSpeed;
        _targetT = FindObjectOfType<PlayerController>().transform;
        UpdateDisplay();
    }

    private void Update()
    {
        _animator.SetBool(_moveAnimKey, _move);
        if (!_move) return;

        Vector3 direction = (_targetT.position - transform.position).normalized;

        transform.Translate(-direction * _bossSpeed * Time.deltaTime);
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * _bossSpeed);
        }
    }

    public void TakeDamage(float damage)
    {
        _health -= damage;
        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
        _bloodFX.Play();
        UpdateDisplay();
    }

    protected override void Die()
    {
        base.Die();
        _move = false;
        GameManager.Instance.GameWin();
    }
}