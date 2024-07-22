
public abstract class SheepStates
{
    public abstract void EnterState(SheepController fsm);
    public abstract void UpdateState(SheepController fsm);
    public abstract void ExitState(SheepController fsm);
}
