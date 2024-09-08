using UnityEngine;

public class AnimalFollowHerdState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        //Debug.Log("FOLLOW HERD " + fsm.gameObject.name);
        //fsm.OnWalk.Invoke();
        fsm.FollowHerd();
        InteractUIController.Instance.ManageInteractUI(InteractType.Accelerate, InteractType.CollectMeat);
    }

    public override void UpdateState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.FollowHerd)
        {
            fsm.CheckIfArrived();
            fsm.FollowHerd();
            fsm.RejoinHerd();
            fsm.SettleInBarn();
            //fsm.CheckDistanceToPlayer();
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.Rest)
            fsm.SwitchState(fsm.restState);
        else if (fsm.executingState == ExecutingAnimalState.Graze)
            fsm.SwitchState(fsm.grazeState);
        else if (fsm.executingState == ExecutingAnimalState.GoToMeadow)
            fsm.SwitchState(fsm.goToMeadowState);
        //else if (fsm.executingState == ExecutingCowState.Flee)
        //    fsm.SwitchState(fsm.fleeState);
        else if (fsm.executingState == ExecutingAnimalState.GetHunted)
            fsm.SwitchState(fsm.getHuntedState);
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {
        
    }
}
