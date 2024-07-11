using UnityEngine;

public class CowFleeState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        Debug.Log("FLEE " + fsm.gameObject.name);
        fsm._herdHeartbeat = fsm._maxDuration;
        fsm.OnWalk.Invoke();
        fsm.StartFlee();
    }

    public override void UpdateState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.Flee)
        {
            fsm.SettleInBarn();
            fsm.Flee();
            fsm.FindNearestHerd();
            fsm.CheckIfArrived();
        }   
        else
            ExitState(fsm);
    }

    public override void ExitState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if(fsm.executingState == ExecutingCowState.FollowHerd)
            fsm.SwitchState(fsm.followHerdState);
    }
}
