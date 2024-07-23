using UnityEngine;

public class CowGetMilkedState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        //Debug.Log("GET MILKED " + fsm.gameObject.name);
        //fsm.InteractableUIElements[0].interactableText = fsm.interactTXT;
        fsm.ChangeUIElement();
    }

    public override void UpdateState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.GetMilked)
        {
            
        }
        else
            ExitState(fsm);
    }

    public override void Interact(CowController fsm, KeyCode interactKey)
    {
        fsm.GetMilked(interactKey);
    }

    public override void ExitState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.DoNothing)
            fsm.SwitchState(fsm.doNothingState);
    }
}
