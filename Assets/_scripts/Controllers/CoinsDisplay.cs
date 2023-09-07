using UnityEngine;
using TMPro;

public class CoinsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txt;

    private void OnEnable()
    {
        EventBus.OnCoinsChange += UpdateDisplay;
        UpdateDisplay();
    }
    private void OnDisable()
    {
        EventBus.OnCoinsChange -= UpdateDisplay;
    }

    private void Start()
    {
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        _txt.text = Coins.Instance.CoinsCount.ToString();
    }
}