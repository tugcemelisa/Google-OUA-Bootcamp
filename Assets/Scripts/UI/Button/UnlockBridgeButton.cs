using System;
using UnityEngine;
using UnityEngine.UI;

public class UnlockBridgeButton : MonoBehaviour
{
    Button button;
    [HideInInspector] public static Action OnUnlockBridgeRequest;

    private void OnEnable()
    {
        button = GetComponent<Button>();

        button.onClick.AddListener(() =>
        {
            OnUnlockBridgeRequest.Invoke();

            MapManager.Instance.OpenMap(false);

            //Sound
            SoundManager.Instance.ChangeAmbientSound(AmbientSoundType.NormalVillage);
        });

    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(() =>
        {
            OnUnlockBridgeRequest.Invoke();

            MapManager.Instance.OpenMap(false);

            //Sound
            SoundManager.Instance.ChangeAmbientSound(AmbientSoundType.NormalVillage);
        });
    }
}
