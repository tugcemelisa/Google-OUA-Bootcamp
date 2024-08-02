using System;
using UnityEngine;

public enum ExecutingGameMode
{
    Daytime,
    Night
}
public class GameModeManager : MonoBehaviourSingletonPersistent<GameModeManager>
{
    [HideInInspector] public ExecutingGameMode executingGameMode;

    [HideInInspector] public static Action OnNightStart;

    private void OnEnable()
    {
        executingGameMode = ExecutingGameMode.Daytime;
    }

    public bool IsDay()
    {
        if (executingGameMode == ExecutingGameMode.Daytime) return true;
        return false;
    }
}
