using UnityEngine;

public class FightStarter : MonoBehaviour, IGameModeChanger
{
    public void ChangeGameMode()
    {
        GameModeManager.OnNightStart.Invoke();
        GameModeManager.Instance.executingGameMode = ExecutingGameMode.Night;
    }
}
