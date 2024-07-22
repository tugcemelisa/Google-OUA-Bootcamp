using UnityEngine;

public class SheepMoveAroundState : SheepStates
{
    public override void EnterState(SheepController fsm)
    {
        Debug.Log("move around");
        fsm._herdHeartbeat = fsm._maxDuration;
        fsm.StartMove();
    }

    public override void UpdateState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.MoveAround)
        {
            fsm.FindNearestHerd();
            fsm.CheckIfArrived();
            fsm.CheckDistanceToPlayer();
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if (fsm.executingState == ExecutingSheepState.Flee)
            fsm.SwitchState(fsm.fleeState);
        else if (fsm.executingState == ExecutingSheepState.FollowHerd)
            fsm.SwitchState(fsm.followHerdState);
    }
}
