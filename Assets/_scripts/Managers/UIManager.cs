using UnityEngine;

public enum PanelType
{
    None,
    Menu,
    Game,
    GameOver,
    LevelComplete,
    Upgrade
}

public class UIManager : MonoBehaviour
{
    [SerializeField] private VirtualPanel[] _panels;

    private void OnEnable()
    {
        EventBus.OnGameStart += OpenGame;
        EventBus.OnLvlComplete += OpenComplete;
        EventBus.OnGameLose += OpenLose;
    }

    private void OnDisable()
    {
        EventBus.OnGameStart -= OpenGame;
        EventBus.OnLvlComplete -= OpenComplete;
        EventBus.OnGameLose -= OpenLose;
    }

    private void Start()
    {
        OpenMenu();
    }

    public void OpenPanel(PanelType type)
    {
        foreach (var panel in _panels)
            panel.gameObject.SetActive(panel.Type == type);
    }

    public void OpenMenu()
    {
        OpenPanel(PanelType.Menu);
    }

    public void OpenUpgrades()
    {
        OpenPanel(PanelType.Upgrade);
    }

    private void OpenGame()
    {
        OpenPanel(PanelType.Game);
    }

    private void OpenComplete()
    {
        OpenPanel(PanelType.LevelComplete);
    }

    private void OpenLose()
    {
        OpenPanel(PanelType.GameOver);
    }
}
