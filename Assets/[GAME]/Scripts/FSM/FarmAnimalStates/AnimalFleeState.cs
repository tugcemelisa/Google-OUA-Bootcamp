using UnityEngine;

public class AnimalFleeState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        //Debug.Log("FLEE " + fsm.gameObject.name);
        fsm._herdHeartbeat = fsm._maxDuration;
        fsm.OnWalk.Invoke();
        fsm.StartFlee();
        Debug.Log("walk in flee " + fsm.Agent.pathEndPosition);
    }

    public override void UpdateState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.Flee)
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

    public override void ExitState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if(fsm.executingState == ExecutingAnimalState.FollowHerd)
            fsm.SwitchState(fsm.followHerdState);
        else if (fsm.executingState == ExecutingAnimalState.GetHunted)
            fsm.SwitchState(fsm.getHuntedState);
        else if(fsm.executingState == ExecutingAnimalState.Rest) 
            fsm.SwitchState(fsm.restState);
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {
        fsm.GetScared();
    }
}
