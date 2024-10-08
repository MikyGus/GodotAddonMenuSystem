﻿using System;

namespace MenySystem.addons.menusystem;

public static class MenuEvents
{
    public static event Action OnGameLevelStart;
    public static event Action OnGameLevelEnd;
    public static event Action OnGamePaused;
    public static event Action OnGameResumed;

    public static void StartGame() => OnGameLevelStart?.Invoke();
    public static void EndGame() => OnGameLevelEnd?.Invoke();
    public static void PauseGame() => OnGamePaused?.Invoke();
    public static void ResumeGame() => OnGameResumed?.Invoke();
}
