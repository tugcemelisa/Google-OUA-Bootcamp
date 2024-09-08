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

        interactUIController.ShowInteractUI(InteractType.E_ForOffer);
        interactUIController.ShowInteractUI(InteractType.AcceptOffer);
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
        interactUIController.HideInteractUI(InteractType.E_ForOffer);
        interactUIController.ManageInteractUI(InteractType.AlreadyAccepted, InteractType.AcceptOffer);
        PlayerInteractableUI.Instance.UpdateUIElements();

        GiveAnimals();
        _executingState = executingState.Wait;

        //Sound
        SoundManager.Instance.PlaySound(VoiceType.Success, null, transform.position);
    }

    public GameObject player;
    IPlayer IPlayer;
    public void GiveAnimals()
    {
        IPlayer = player.GetComponent<IPlayer>();
        IPlayer.TakeAnimals(herdAnimals);
    }
}
