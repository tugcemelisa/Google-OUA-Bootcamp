using System;
using UnityEngine;
using UnityEngine.UI;

public class GoMeadowButton : MonoBehaviour
{
    Button button;
    [HideInInspector] public static Action<MeadowWolfManagerInfoHolder> OnGoingMeadowRequest;

    [SerializeField] private MeadowWolfManagerInfoHolder meadowWolfManagerInfoHolder;

    private void OnEnable()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            OnGoingMeadowRequest.Invoke(meadowWolfManagerInfoHolder);
            MapManager.Instance.OpenMap(false);
        });
    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(() =>
        {
            OnGoingMeadowRequest.Invoke(meadowWolfManagerInfoHolder);
            MapManager.Instance.OpenMap(false);
        });
    }
}
