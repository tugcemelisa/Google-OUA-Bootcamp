using UnityEngine;

public class AnimalGetHuntedState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        fsm.OnWalk.Invoke();
        fsm.Agent.SetDestination(fsm.GetRandomPos(fsm.meadow.position, 5f));
    }

    public override void UpdateState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.GetHunted)
        {
            if (fsm.Agent.remainingDistance <= 3.60f)
            {
                fsm.Agent.radius = 0.5f;
                fsm.gameObject.GetComponent<Collider>().enabled = false;    // !!!!!
                fsm.Agent.SetDestination(fsm.transform.position);
                fsm.OnIdle.Invoke();
                fsm.executingState = ExecutingAnimalState.DoNothing;
            }
        }
        else if (fsm.executingState == ExecutingAnimalState.Dead)
        {
            fsm.SwitchState(fsm.deadState);
        }
        else
            GetHunted(fsm);
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {

    }

    public override void ExitState(AnimalBase fsm)
    {

    }

    private void GetHunted(AnimalBase fsm)
    {
        fsm.Agent.SetDestination(fsm.transform.position);   //!!!!!!!!!!!!
    }
}
