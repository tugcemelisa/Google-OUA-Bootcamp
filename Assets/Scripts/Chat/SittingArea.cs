using Cinemachine;
using System;
using UnityEngine;

public class SittingArea : Interactable
{
    [HideInInspector] public static Action OnPlayerSit;

    [SerializeField] PlayerSimulationController player;
    [SerializeField] CinemachineVirtualCamera virtualCamera;

    [SerializeField] float forwardingTime = 7f;
    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        if (interactKey == (KeyCode)InteractKeys.Sit)
        {
            foreach (var item in InteractableUIElements)
            {
                if ((int)item.InteractKey == (int)InteractKeys.Sit && item.enabled)
                {
                    print(item);
                    item.Disable(true);
                }
            }

            if (player)
                SitDown();
        }
    }

    void SitDown()
    {
        StarterAssets.InputController.Instance.DisableInputs();

        player.SitToTheGround();
        virtualCamera.Priority = 100;
        OnPlayerSit?.Invoke();
        TimePass();
    }

    void TimePass()
    {
        
        //TimeManager.Instance.PassTimeUntilDark();
        Invoke("StandUp", forwardingTime);
    }

    void StandUp()
    {
        player.SitToTheGround();
        virtualCamera.Priority = 1;

        InventoryManager.BeforeDayNightCycle.Invoke();
        GameModeManager.OnNightStart.Invoke();

        //Sound
        SoundManager.Instance.ChangeAmbientSound(AmbientSoundType.NightAmbient);
        SoundManager.Instance.PlaySound(VoiceType.WolfHowling,transform,transform.position);
    }
}
