using System;
using UnityEngine;
using UnityEngine.UI;

public class ReturnVillageButton : MonoBehaviour
{
    Button button;
    [HideInInspector] public static Action OnReturnVillageRequest;

    private void OnEnable()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() => OnReturnVillageRequest.Invoke());
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(() => OnReturnVillageRequest.Invoke());
    }
}
