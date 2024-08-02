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

        button.onClick.AddListener(() =>
        {
            OnReturnVillageRequest.Invoke();

            MapManager.Instance.OpenMap(false);

            //Sound
            SoundManager.Instance.ChangeAmbientSound(AmbientSoundType.NormalVillage);
        });

    }
    private void OnDisable()
    {
        button.onClick.RemoveListener(() =>
        {
            OnReturnVillageRequest.Invoke();

            MapManager.Instance.OpenMap(false);

            //Sound
            SoundManager.Instance.ChangeAmbientSound(AmbientSoundType.NormalVillage);
        });
    }
}
