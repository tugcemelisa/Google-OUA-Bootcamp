using UnityEngine;

public class CowGetMilkedState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        //Debug.Log("GET MILKED " + fsm.gameObject.name);
    }

    public override void UpdateState(CowController fsm)
    {
        if (fsm.executingState == ExecutingCowState.GetMilked)
        {
            fsm.GetMilked();
        }
        //else
        //    ExitState(fsm);
    }

    public override void ExitState(CowController fsm)
    {

    }
}
