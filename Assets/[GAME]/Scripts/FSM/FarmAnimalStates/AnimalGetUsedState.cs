using UnityEngine;

public class AnimalGetUsedState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        //Debug.Log("GET MILKED " + fsm.gameObject.name);
        //fsm.InteractableUIElements[0].interactableText = fsm.interactTXT;
        fsm.GetUsedInteractUI();
        HelperController.Instance.ShowHelper(HelpType.FindClient);
        fsm.Agent.radius = 0.5f;
    }

    public override void UpdateState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.GetUsed)
        {
            
        }
        else
            ExitState(fsm);
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {
        fsm.GetUsed(interactKey);
    }

    public override void ExitState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.DoNothing)
            fsm.SwitchState(fsm.doNothingState);
    }
}
