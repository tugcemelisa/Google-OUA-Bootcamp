using UnityEngine;

public class SittingArea : Interactable
{
    [SerializeField] PlayerSimulationController player;
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
        print("sit");
        player.SitToTheGround();
    }

    void TimePass()
    {

    }

    void StandUp()
    {

    }
}
