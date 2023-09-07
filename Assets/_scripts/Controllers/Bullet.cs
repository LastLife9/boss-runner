using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float _shootingDamage;
    private float _bulletSpeed;
    private float _lifetime = 5f;
    private float _timer = 0f;

    private void Start()
    {
        _shootingDamage = GameConfig.Instance.GameParameters.damagePerShot;
        _bulletSpeed = GameConfig.Instance.GameParameters.bulletSpeed;
        _lifetime = GameConfig.Instance.GameParameters.bulletLifetime;
        _timer = _lifetime;
    }

    private void Update()
    {
        if(_timer <= 0)
        {
            Disable();
            return;
        }
        _timer -= Time.deltaTime;

        transform.Translate(transform.forward * _bulletSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamagable damagable))
        {
            damagable.TakeDamage(_shootingDamage);
            Disable();
        }
    }

    private void Disable()
    {
        _timer = _lifetime;
        gameObject.SetActive(false);
    }
}