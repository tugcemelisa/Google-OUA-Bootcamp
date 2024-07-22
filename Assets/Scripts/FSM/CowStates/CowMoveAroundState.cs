using UnityEngine;

public class CowMoveAroundState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        //Debug.Log("MOVE AROUND " + fsm.gameObject.name);
        fsm._herdHeartbeat = fsm._maxDuration;
        fsm.StartMove();
    }

    public override void UpdateState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.MoveAround)
        {
            fsm.FindNearestHerd();
            fsm.CheckIfArrived();
            fsm.CheckDistanceToPlayer();
            //fsm.SettleInBarn();
        }   
        else
            ExitState(fsm);
    }

    public override void ExitState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if (fsm.executingState == ExecutingCowState.Flee)
            fsm.SwitchState(fsm.fleeState);
        else if (fsm.executingState == ExecutingCowState.FollowHerd)
            fsm.SwitchState(fsm.followHerdState);
    }
}
