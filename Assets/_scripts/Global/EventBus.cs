using System;

public static class EventBus
{
    public static Action<GameState> OnChangeState { get; set; }
    public static Action OnGameStart { get; set; }
    public static Action OnGameLose { get; set; }
    public static Action OnLvlComplete { get; set; }
    public static Action OnGamePause { get; set; }
    public static Action OnTrackSegmentPassed { get; set; }
    public static Action OnBossSpawn { get; set; }
    public static Action OnBossFightStart { get; set; }
    public static Action OnEnemyKill { get; set; }
    public static Action OnCoinsChange { get; set; }
}