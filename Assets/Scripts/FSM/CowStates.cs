using UnityEngine;

public abstract class CowStates
{
    public abstract void EnterState(CowController fsm);
    public abstract void UpdateState(CowController fsm);
    public abstract void ExitState(CowController fsm);
}