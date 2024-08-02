using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    public void StartGame()
    {
        MenuManager.Instance.StartGameMenu(false);
    }
}
