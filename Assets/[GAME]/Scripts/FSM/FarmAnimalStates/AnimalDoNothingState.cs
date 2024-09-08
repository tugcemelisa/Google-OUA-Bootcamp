using UnityEngine;

public class AnimalDoNothingState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        //fsm.ChangeUIElement();
        fsm.OnIdle.Invoke();
        fsm.DoNothingInteractUI();
    }

    public override void UpdateState(AnimalBase fsm)
    {
        
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {
        
    }

    public override void ExitState(AnimalBase fsm)
    {
        //Do this when needed, in fsm for both animals use (shear and milk).
        //InteractUIController.Instance.HideInteractUI(InteractType.AlreadyMilked);
    }
}
