using UnityEngine;

public class CowGetHuntedState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        fsm.OnWalk.Invoke();
        fsm.Agent.SetDestination(fsm.GetRandomPos(fsm.meadow.position, 5f));
    }

    public override void UpdateState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.GetHunted)
        {
            if (fsm.Agent.remainingDistance <= 3.60f)
            {
                fsm.Agent.radius = 0.5f;    
                fsm.gameObject.GetComponent<Collider>().enabled = false;    // !!!!!
                fsm.Agent.SetDestination(fsm.transform.position);
                fsm.OnIdle.Invoke();
                fsm.executingState = ExecutingCowState.DoNothing;
            }
        }
        else
            GetHunted(fsm);
    }

    public override void Interact(CowController fsm, KeyCode interactKey)
    {
        
    }

    public override void ExitState(CowController fsm)
    {
        
    }

    private void GetHunted(CowController fsm)
    {
        fsm.Agent.SetDestination(fsm.transform.position);   //!!!!!!!!!!!!
    }
}
