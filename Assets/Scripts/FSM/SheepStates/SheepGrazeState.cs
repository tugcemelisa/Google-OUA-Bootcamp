using UnityEngine;

public class SheepGrazeState : SheepStates
{
    public override void EnterState(SheepController fsm)
    {
        Debug.Log("graze");
        fsm.OnGraze.Invoke();
        fsm.Agent.SetDestination(fsm.transform.position);
        fsm._grazeTimer = fsm.grazeTime;
    }

    public override void UpdateState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.Graze)
        {
            fsm.Graze();
            fsm.CheckDistanceToPlayer();
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.MoveAround)
            fsm.SwitchState(fsm.moveAroundState);
        else if (fsm.executingState == ExecutingSheepState.Flee)
            fsm.SwitchState(fsm.fleeState);
    }
}
