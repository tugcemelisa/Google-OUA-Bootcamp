using UnityEngine;
using UnityEngine.UI;

public class ReturnVillageButton : MonoBehaviour
{
    Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => WolfManager.OnHuntOver.Invoke());
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(() => WolfManager.OnHuntOver.Invoke());
    }
}
