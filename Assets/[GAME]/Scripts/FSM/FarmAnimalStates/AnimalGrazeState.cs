using System;
using UnityEngine;

public class AnimalGrazeState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        //Debug.Log("GRAZE " + fsm.gameObject.name + "\n " + fsm._grazeTimer);
        ChooseAnim(fsm);
        fsm.Agent.SetDestination(fsm.transform.position);
        fsm._grazeTimer = fsm.grazeTime;
        InteractUIController.Instance.ManageInteractUI(InteractType.Accelerate, InteractType.AlreadyMilked);
    }

    public override void UpdateState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.Graze)
        {
            fsm.Graze();
            fsm.CheckDistanceToPlayer();
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.MoveAround)
            fsm.SwitchState(fsm.moveAroundState);
        else if (fsm.executingState == ExecutingAnimalState.Flee)
            fsm.SwitchState(fsm.fleeState);
        else if (fsm.executingState == ExecutingAnimalState.GetHunted)
            fsm.SwitchState(fsm.getHuntedState);
    }

    private void ChooseAnim(AnimalBase fsm)
    {
        int stateIndex = UnityEngine.Random.Range(0, 2);
        if(stateIndex ==  0)
            fsm.OnGraze.Invoke();
        else if(stateIndex == 1)
            fsm.OnIdle.Invoke();
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {
        
    }
}
