using UnityEngine;

public class CowFleeState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        //Debug.Log("FLEE " + fsm.gameObject.name);
        fsm._herdHeartbeat = fsm._maxDuration;
        fsm.OnWalk.Invoke();
        fsm.StartFlee();
        Debug.Log("walk in flee " + fsm.Agent.pathEndPosition);
    }

    public override void UpdateState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.Flee)
        {
            fsm.Flee();
            //fsm.FindNearestHerd();
            fsm.CheckIfArrived();
            fsm.FindNearestHerd();
            //if (fsm.Agent.remainingDistance <= 4.1f)
            //{
            //    fsm.executingState = ExecutingCowState.Graze;
            //}
            fsm.SettleInBarn();
        }   
        else
            ExitState(fsm);
    }

    public override void ExitState(CowController fsm)
    {
        fsm.Agent.acceleration = fsm.acceleration;
        fsm.Agent.speed = fsm.speed;

        if (fsm.executingState == ExecutingCowState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if(fsm.executingState == ExecutingCowState.FollowHerd)
            fsm.SwitchState(fsm.followHerdState);
        else if (fsm.executingState == ExecutingCowState.GetHunted)
            fsm.SwitchState(fsm.getHuntedState);
        else if(fsm.executingState == ExecutingCowState.Rest) 
            fsm.SwitchState(fsm.restState);
    }

    public override void Interact(CowController fsm, KeyCode interactKey)
    {
        
    }
}
