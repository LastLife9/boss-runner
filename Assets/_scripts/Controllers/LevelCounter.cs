using UnityEngine;
using TMPro;

public class LevelCounter : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _txt;

    private void Start()
    {
        _txt.text = (GameConfig.Instance.GameParameters.completedLevelsCount + 1).ToString();
    }
}
