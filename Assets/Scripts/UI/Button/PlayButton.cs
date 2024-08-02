using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    [SerializeField] PlayerSimulationController player;
    public void StartGame()
    {
        MenuManager.Instance.PlayButton();
    }

    public void Restart()
    {
        PlayerPrefs.SetFloat("Money", player.TotalAmount);
        SceneManager.LoadScene(0);
    }
}
