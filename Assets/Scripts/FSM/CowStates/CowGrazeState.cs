using System;
using UnityEngine;

public class CowGrazeState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        Debug.Log("GRAZE " + fsm.gameObject.name + "\n " + fsm._grazeTimer);
        ChooseAnim(fsm);
        fsm.Agent.SetDestination(fsm.transform.position);
        fsm._grazeTimer = fsm.grazeTime;
    }

    public override void UpdateState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.Graze)
        {
            fsm.Graze();
            fsm.CheckDistanceToPlayer();
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.MoveAround)
            fsm.SwitchState(fsm.moveAroundState);
        else if (fsm.executingState == ExecutingCowState.Flee)
            fsm.SwitchState(fsm.fleeState);
    }

    private void ChooseAnim(CowController fsm)
    {
        int stateIndex = UnityEngine.Random.Range(0, 2);
        if(stateIndex ==  0)
            fsm.OnGraze.Invoke();
        else if(stateIndex == 1)
            fsm.OnIdle.Invoke();
    }
}
