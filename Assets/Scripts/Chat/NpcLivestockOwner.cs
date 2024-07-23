using System.Collections.Generic;
using UnityEngine;

public class NpcLivestockOwner : NPCInteractable
{
    enum executingState
    {
        LookForShepherd,
        Wait
    }
    executingState _executingState;
    [SerializeField] List<AnimalBase> herdAnimals = new ();
    [SerializeField] int AmountToBePaid;

    public override void Start()
    {
        base.Start();
        _executingState = executingState.LookForShepherd;
    }
    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        if (_executingState == executingState.Wait)
            return;
        else if (_executingState == executingState.LookForShepherd)
        {
            if ((int)interactKey == (int)InteractKeys.Accept)
            { AcceptQuest();  }

            else if ((int)interactKey == (int)InteractKeys.Talk)
            {
                Talk(interactorTransform, IconType.Neutral, "I have " + herdAnimals.Count + " to be grazed and " +
                    AmountToBePaid + " dollars.\nDo you want to do it?");
            }
        }
    }

    private void AcceptQuest()
    {
        foreach (var item in InteractableUIElements)
        {
            if (item.enabled && item.InteractKey == InteractKeys.Talk)
            {
                item.enabled = false;
                PlayerInteractableUI.Instance.UpdateUIElements();
            }
            if (item.enabled && item.InteractKey == InteractKeys.Accept)
            {
                item.Disable(true);
                PlayerInteractableUI.Instance.UpdateUIElements();
                break;
            }
        }

        _executingState = executingState.Wait;
    }
}
