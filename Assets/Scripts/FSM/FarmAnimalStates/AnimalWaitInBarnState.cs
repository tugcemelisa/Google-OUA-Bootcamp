using UnityEngine;

public class AnimalWaitInBarnState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        fsm.OnIdle.Invoke();
        fsm.Agent.radius = 0.5f;
    }

    public override void UpdateState(AnimalBase fsm)
    {
        if(fsm.executingState == ExecutingAnimalState.WaitInBarn)
        {

        }
        else
            ExitState(fsm);
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {
        
    }

    public override void ExitState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.GoToMeadow)
            fsm.SwitchState(fsm.goToMeadowState);
    }
}
