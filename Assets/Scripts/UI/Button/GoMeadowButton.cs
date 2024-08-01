using UnityEngine;
using UnityEngine.UI;

public class GoMeadowButton : MonoBehaviour
{
    Button button;

    private void OnEnable()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => PlayerSimulationController.OnHerdLeaveBarn.Invoke());
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(() => PlayerSimulationController.OnHerdLeaveBarn.Invoke());
    }
}
