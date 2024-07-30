using Cinemachine;
using UnityEngine;

public class SittingArea : Interactable
{

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
        player.SitToTheGround();
        virtualCamera.Priority = 100;
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
    }
}
