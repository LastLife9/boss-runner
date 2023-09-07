public class Enemy : EnemyBase, IDamagable
{
    private void Start()
    {
        _health = GameConfig.Instance.GameParameters.enemyHealth;
        UpdateDisplay();
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
        EventBus.OnEnemyKill?.Invoke();
    }
}
