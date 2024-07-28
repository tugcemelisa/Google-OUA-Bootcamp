using UnityEngine;

public class FightStarter : MonoBehaviour, IGameModeChanger
{
    public void ChangeGameMode()
    {
        InventoryManager.BeforeDayNightCycle.Invoke();
        GameModeManager.OnNightStart.Invoke();
    }
}
