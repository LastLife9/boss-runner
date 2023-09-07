public class Coins : Singleton<Coins>
{
    public int CoinsCount { get; private set; }

    private void OnEnable()
    {
        EventBus.OnEnemyKill += () => 
        { 
            AddCoins(GameConfig.Instance.GameParameters.coinsPerKill); 
        };
        EventBus.OnLvlComplete += () =>
        {
            AddCoins(GameConfig.Instance.GameParameters.lvlCompleteReward);
        };
    }
    private void OnDisable()
    {
        EventBus.OnEnemyKill -= () =>
        {
            AddCoins(GameConfig.Instance.GameParameters.coinsPerKill);
        };
        EventBus.OnLvlComplete -= () =>
        {
            AddCoins(GameConfig.Instance.GameParameters.lvlCompleteReward);
        };
    }

    private void Start()
    {
        CoinsCount = GameConfig.Instance.GameParameters.coinsCount;
    }

    public void AddCoins(int count)
    {
        CoinsCount += count;
        UpdParam();
    }

    public void RemoveCoins(int count)
    {
        CoinsCount -= count;
        UpdParam();
    }

    private void UpdParam()
    {
        GameConfig.Instance.GameParameters.coinsCount = CoinsCount;
        GameConfig.Instance.SaveParameters();
        EventBus.OnCoinsChange?.Invoke();
    }
}
