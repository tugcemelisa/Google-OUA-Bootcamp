using UnityEngine;

public class SheepFollowHerdState : SheepStates
{
    public override void EnterState(SheepController fsm)
    {
        Debug.Log("follow herd");
        fsm.FollowHerd();
    }

    public override void UpdateState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.FollowHerd)
        {
            fsm.SettleInBarn();
            fsm.CheckIfArrived();
            fsm.CheckDistanceToPlayer();
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.Rest)
            fsm.SwitchState(fsm.restState);
        else if (fsm.executingState == ExecutingSheepState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if (fsm.executingState == ExecutingSheepState.Flee)
            fsm.SwitchState(fsm.fleeState);
    }
}
