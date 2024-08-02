using UnityEngine;

public class AnimalMoveAroundState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        //Debug.Log("MOVE AROUND " + fsm.gameObject.name);
        fsm._herdHeartbeat = fsm._maxDuration;
        fsm.StartMove();
    }

    public override void UpdateState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.MoveAround)
        {
            fsm.FindNearestHerd();
            fsm.CheckIfArrived();
            fsm.CheckDistanceToPlayer();
            //fsm.SettleInBarn();
        }   
        else
            ExitState(fsm);
    }

    public override void ExitState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if (fsm.executingState == ExecutingAnimalState.Flee)
            fsm.SwitchState(fsm.fleeState);
        else if (fsm.executingState == ExecutingAnimalState.FollowHerd)
            fsm.SwitchState(fsm.followHerdState);
        else if (fsm.executingState == ExecutingAnimalState.GetHunted)
            fsm.SwitchState(fsm.getHuntedState);
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {
        
    }
}
