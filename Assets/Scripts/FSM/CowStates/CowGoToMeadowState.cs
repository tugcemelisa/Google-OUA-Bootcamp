using UnityEngine;

public class CowGoToMeadowState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        fsm.OnWalk.Invoke();
        fsm.Agent.SetDestination(fsm.GetRandomPos(fsm.meadow.position, 20f));
    }

    public override void UpdateState(CowController fsm)
    {
        if(fsm.executingState == ExecutingCowState.GoToMeadow)
        {
            Debug.Log(fsm.Agent.destination + " remainingDistance: " + fsm.Agent.remainingDistance
                + "\n meadow.position: " + fsm.meadow.position);
            
            if(fsm.Agent.hasPath && fsm.Agent.remainingDistance <= fsm.Agent.stoppingDistance)    // !!!!!!S
            {
                Debug.Log("animal reached");
                fsm.executingState = ExecutingCowState.Graze;
            }
        }
        else
            ExitState(fsm);
    }

    public override void Interact(CowController fsm, KeyCode interactKey)
    {
        fsm.GetScared();
    }

    public override void ExitState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.Flee)
        {
            FarmAnimalManager.Instance.Straggle();
            fsm.SwitchState(fsm.fleeState);
        }  
        else if (fsm.executingState == ExecutingCowState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if (fsm.executingState == ExecutingCowState.GetHunted)
            fsm.SwitchState(fsm.getHuntedState);
    }
}
