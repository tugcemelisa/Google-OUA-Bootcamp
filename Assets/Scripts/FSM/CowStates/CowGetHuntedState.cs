using UnityEngine;

public class CowGetHuntedState : CowStates
{
    public override void EnterState(CowController fsm)
    {
        fsm.Agent.radius = 0.5f;    // wait until all the animals settle in the barn.
        fsm.gameObject.GetComponent<Collider>().enabled = false;    // !!!!!
        fsm.Agent.SetDestination(fsm.transform.position);
        //fsm.OnIdle.Invoke();
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
