using System.Collections;
using UnityEngine;

public class PlayerDeathObserver : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            GameManager.Instance.GameLose();
        }
    }
}