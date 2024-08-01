using System;
using UnityEngine;
using UnityEngine.UI;

public class GoMeadowButton : MonoBehaviour
{
    Button button;
    [HideInInspector] public static Action OnGoingMeadowRequest;

    private void OnEnable()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            OnGoingMeadowRequest.Invoke();
            MapManager.Instance.OpenMap(false);
        });
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(() =>
        {
            OnGoingMeadowRequest.Invoke();
            MapManager.Instance.OpenMap(false);
        });
    }
}
