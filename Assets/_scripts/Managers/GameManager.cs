using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private GameState _currentState = GameState.None;
    public GameState CurrentState
    {
        get => _currentState;
    }

    protected override void Awake()
    {
        base.Awake();

        Application.targetFrameRate = 60;
    }

    private void ChangeState(GameState _newState)
    {
        _currentState = _newState;
        EventBus.OnChangeState?.Invoke(_currentState);
    }

    public void GameStart()
    {
        ChangeState(GameState.Game);
        EventBus.OnGameStart?.Invoke();
    }
    public void GameLose()
    {
        if (CurrentState == GameState.End) return;

        ChangeState(GameState.End);
        EventBus.OnGameLose?.Invoke();
    }
    public void GameWin()
    {
        if (CurrentState == GameState.End) return;
        GameConfig.Instance.GameParameters.completedLevelsCount++;
        GameConfig.Instance.SaveParameters();
        ChangeState(GameState.End);
        EventBus.OnLvlComplete?.Invoke();
    }
    public void GameReload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
