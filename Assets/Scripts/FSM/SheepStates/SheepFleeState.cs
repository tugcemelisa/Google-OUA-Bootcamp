using UnityEngine;

public class SheepFleeState : SheepStates
{
    public override void EnterState(SheepController fsm)
    {
        Debug.Log("Flee");
        fsm.OnRun.Invoke();
        fsm.StartFlee();
    }

    public override void UpdateState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.Flee) 
        {
            fsm.Flee();
            fsm.CheckIfArrived();
            fsm.SettleInBarn();
            fsm.FindNearestHerd();
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if (fsm.executingState == ExecutingSheepState.FollowHerd)
            fsm.SwitchState(fsm.followHerdState);
        else if (fsm.executingState == ExecutingSheepState.Rest)
            fsm.SwitchState(fsm.restState);
    }
}
