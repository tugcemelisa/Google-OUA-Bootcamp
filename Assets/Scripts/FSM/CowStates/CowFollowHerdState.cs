using UnityEngine;

public class CowFollowHerdState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        //Debug.Log("FOLLOW HERD " + fsm.gameObject.name);
        //fsm.OnWalk.Invoke();
        fsm.FollowHerd();
    }

    public override void UpdateState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.FollowHerd)
        {
            fsm.SettleInBarn();
            fsm.CheckIfArrived();
            fsm.CheckDistanceToPlayer();
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.Rest)
            fsm.SwitchState(fsm.restState);
        else if (fsm.executingState == ExecutingCowState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if (fsm.executingState == ExecutingCowState.Flee)
            fsm.SwitchState(fsm.fleeState);
        else if (fsm.executingState == ExecutingCowState.GetHunted)
            fsm.SwitchState(fsm.getHuntedState);
    }

    public override void Interact(CowController fsm, KeyCode interactKey)
    {
        
    }
}
