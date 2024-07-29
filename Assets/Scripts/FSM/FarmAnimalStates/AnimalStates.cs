using UnityEngine;

public abstract class AnimalStates
{
    public abstract void EnterState(AnimalBase fsm);
    public abstract void UpdateState(AnimalBase fsm);
    public abstract void Interact(AnimalBase fsm, KeyCode interactKey);
    public abstract void ExitState(AnimalBase fsm);
}
