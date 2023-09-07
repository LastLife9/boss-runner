using System;

[Serializable]
public class GameParameters
{
    public float bulletSpeed = 10f;
    public float bulletLifetime = 4f;

    public float delayBetweenEnemies = 10f;
    public float enemyHealth = 5f;

    public float timeToSpawnBoss = 30f;
    public float bossHealth = 20f;
    public float bossSpeed = 2f;

    public int coinsPerKill = 3;
    public int lvlCompleteReward = 100;
    public int coinsCount = 0;
    public int completedLevelsCount = 0;

    public float shootingSpeed = 1f;
    public float damagePerShot = 1f;
    public int damageUpCount = 0;
    public int fireRateUpCount = 0;
}