using TMPro;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    protected float _health;
    [SerializeField] protected TextMeshPro _healthTxt;
    [SerializeField] protected ParticleSystem _bloodFX;
    [SerializeField, Header("Skin Settings")] protected GameObject _baseSkin;
    [SerializeField] protected GameObject _ragdollSkin;
    [SerializeField] protected Rigidbody[] _ragdollRbs;
    [SerializeField] protected float _kickForce = 50f;

    protected virtual void Die()
    {
        _baseSkin.SetActive(false);
        _ragdollSkin.SetActive(true);

        foreach (Rigidbody rb in _ragdollRbs)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(Vector3.forward * _kickForce, ForceMode.VelocityChange);
        }

        _healthTxt.enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    protected void UpdateDisplay()
    {
        _healthTxt.text = _health.ToString("0.0");
    }
}