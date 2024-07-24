using UnityEngine;

public class CowDoNothingState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        //fsm.ChangeUIElement();
    }

    public override void UpdateState(CowController fsm)
    {
        fsm.Agent.SetDestination(fsm.transform.position);   //!!!!!!!!!!!!
    }

    public override void Interact(CowController fsm, KeyCode interactKey)
    {
        
    }

    public override void ExitState(CowController fsm)
    {
        
    }
}
