using UnityEngine;

public class CowWaitInBarnState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        fsm.OnIdle.Invoke();
    }

    public override void UpdateState(CowController fsm)
    {
        if(fsm.executingState == ExecutingCowState.WaitInBarn)
        {

        }
        else
            ExitState(fsm);
    }

    public override void Interact(CowController fsm, KeyCode interactKey)
    {
        
    }

    public override void ExitState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.GoToMeadow)
            fsm.SwitchState(fsm.goToMeadowState);
    }
}
