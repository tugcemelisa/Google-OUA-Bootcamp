using UnityEngine;

public class SheepGetShearedState : SheepStates
{
    public override void EnterState(SheepController fsm)
    {
        Debug.Log("get sheared");
        fsm.OnSit.Invoke();
        fsm.ChangeUIElement();
    }

    public override void UpdateState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.GetSheared)
        {
            
        }
        else
            ExitState(fsm);
    }

    public override void Interact(SheepController fsm, KeyCode interactKey)
    {
        fsm.GetSheared(interactKey);
    }

    public override void ExitState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.DoNothing)
            fsm.SwitchState(fsm.doNothingState);
    }

    
}
