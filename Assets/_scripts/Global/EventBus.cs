using System;

public static class EventBus
{
    public static Action<GameState> OnChangeState { get; set; }
    public static Action OnGameStart { get; set; }
    public static Action OnGameLose { get; set; }
    public static Action OnLvlComplete { get; set; }
    public static Action OnGamePause { get; set; }
    public static Action OnTrackSegmentPassed { get; set; }
}