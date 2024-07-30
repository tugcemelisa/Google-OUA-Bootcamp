
using UnityEngine;

public class AnimalDeadState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        fsm.ShowDeadInteractable();
        fsm.Agent.speed = 0;
    }

    public override void ExitState(AnimalBase fsm)
    {
        
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {
        fsm.SpawnItem();
    }

    public override void UpdateState(AnimalBase fsm)
    {
        //if (fsm.executingState == ExecutingAnimalState.Dead)
        //{

        //}
        //else ExitState(fsm);
    }
}