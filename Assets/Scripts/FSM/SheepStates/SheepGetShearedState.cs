using UnityEngine;

public class SheepGetShearedState : SheepStates
{
    public override void EnterState(SheepController fsm)
    {
        Debug.Log("get sheared");
        fsm.OnSit.Invoke();
    }

    public override void UpdateState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.GetSheared)
        {
            fsm.GetSheared();
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(SheepController fsm)
    {
        
    }
}
