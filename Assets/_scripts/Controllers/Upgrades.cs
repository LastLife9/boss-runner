using UnityEngine;
using TMPro;

public class Upgrades : MonoBehaviour
{
    [SerializeField] private int _startingPrice = 10;
    [SerializeField] private float _multiplier = 1.2f;
    [SerializeField] private float _maxFireRate = 10f;
    [SerializeField] private float _maxDamage = 100f;

    [SerializeField] private TextMeshProUGUI _dmgPriceTxt;
    [SerializeField] private TextMeshProUGUI _fireRatePriceTxt;
    [SerializeField] private TextMeshProUGUI _dmgValueTxt;
    [SerializeField] private TextMeshProUGUI _fireRateValueTxt;

    private float _dmgPrice = 0f;
    private float _fireRatePrice = 0f;
    private int _dmgUpgradeCount = 0;
    private int _fireRateUpgradeCount = 0;

    private void Start()
    {
        _dmgUpgradeCount = GameConfig.Instance.GameParameters.damageUpCount;
        _fireRateUpgradeCount = GameConfig.Instance.GameParameters.fireRateUpCount;

        CalculateParamsDamage();
        CalculateParamsFireRate();
    }

    public void UpgradeDamage()
    {
        if (GameConfig.Instance.GameParameters.damagePerShot == _maxDamage) return;
        _dmgPrice = CalculateUpgradePrice(_dmgUpgradeCount);
        if (Coins.Instance.CoinsCount < _dmgPrice)
        {
            return;
        }

        Coins.Instance.RemoveCoins((int)_dmgPrice);
        _dmgUpgradeCount++;
        GameConfig.Instance.GameParameters.damageUpCount = _dmgUpgradeCount;
        GameConfig.Instance.SaveParameters();
        CalculateParamsDamage();
    }

    public void UpgradeFireRate()
    {
        if (GameConfig.Instance.GameParameters.shootingSpeed == _maxFireRate) return;
        _fireRatePrice = CalculateUpgradePrice(_fireRateUpgradeCount);
        if (Coins.Instance.CoinsCount < _fireRatePrice)
        {
            return;
        }

        Coins.Instance.RemoveCoins((int)_fireRatePrice);
        _fireRateUpgradeCount++;
        GameConfig.Instance.GameParameters.fireRateUpCount = _fireRateUpgradeCount;
        GameConfig.Instance.SaveParameters();
        CalculateParamsFireRate();
    }

    private float CalculateUpgradePrice(int upgradeCount)
    {
        return _startingPrice * Mathf.Pow(_multiplier, upgradeCount);
    }

    private void CalculateParamsDamage()
    {
        _dmgPrice = CalculateUpgradePrice(_dmgUpgradeCount);
        float defaultDamage = GameConfig.Instance.GameParameters.damagePerShot;
        float calculatedDamage = defaultDamage * Mathf.Pow(_multiplier, _dmgUpgradeCount);
        calculatedDamage = Mathf.Min(calculatedDamage, _maxDamage);
        GameConfig.Instance.GameParameters.damagePerShot = calculatedDamage;
        GameConfig.Instance.SaveParameters();
        UpdateDisplay();
    }

    private void CalculateParamsFireRate()
    {
        _fireRatePrice = CalculateUpgradePrice(_fireRateUpgradeCount);
        float defaultFireRate = GameConfig.Instance.GameParameters.shootingSpeed;
        float calculatedFireRate = defaultFireRate / Mathf.Pow(_multiplier, _fireRateUpgradeCount);
        calculatedFireRate = Mathf.Max(calculatedFireRate, _maxFireRate);
        GameConfig.Instance.GameParameters.shootingSpeed = calculatedFireRate;
        GameConfig.Instance.SaveParameters();
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        _dmgPriceTxt.text = _dmgPrice.ToString("0.0");
        _fireRatePriceTxt.text = _fireRatePrice.ToString("0.0");
        _dmgValueTxt.text = GameConfig.Instance.GameParameters.damagePerShot.ToString("0.0");
        _fireRateValueTxt.text = GameConfig.Instance.GameParameters.shootingSpeed.ToString("0.0");
    }
}
