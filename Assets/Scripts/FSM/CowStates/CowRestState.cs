using System;
using UnityEngine;
using UnityEngine.AI;

public class CowRestState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        Debug.Log("REST " + fsm.gameObject.name);
    }

    public override void UpdateState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.Rest)
        {
            if (fsm.Agent.remainingDistance <= fsm.Agent.stoppingDistance)
            {
                fsm.Agent.SetDestination(fsm.transform.position);
                fsm.OnIdle.Invoke();
                fsm.executingState = ExecutingCowState.GetMilked;
            } 
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.GetMilked)
            fsm.SwitchState(fsm.getMilkedState);
    }
}
