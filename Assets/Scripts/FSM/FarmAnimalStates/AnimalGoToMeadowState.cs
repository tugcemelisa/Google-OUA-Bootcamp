using UnityEngine;

public class AnimalGoToMeadowState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        fsm.OnWalk.Invoke();
        fsm.Agent.SetDestination(fsm.GetRandomPos(AnimalManager.Instance.Meadow.position, 20f));
        fsm.gameObject.GetComponent<Collider>().enabled = true;
    }

    public override void UpdateState(AnimalBase fsm)
    {
        if(fsm.executingState == ExecutingAnimalState.GoToMeadow)
        {
            if(fsm.Agent.hasPath && fsm.Agent.remainingDistance <= 3.60f)    
            {
                fsm.executingState = ExecutingAnimalState.Graze;
            }
        }
        else
            ExitState(fsm);
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {
        fsm.GetScared();
    }

    public override void ExitState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.Flee)
        {
            FarmAnimalManager.Instance.Straggle();
            fsm.SwitchState(fsm.fleeState);
        }  
        else if (fsm.executingState == ExecutingAnimalState.Graze)
        {
            HelperController.Instance.ShowHelper(HelpType.REstPanel);
            fsm.SwitchState(fsm.grazeState);
        }
        else if (fsm.executingState == ExecutingAnimalState.GetHunted)
            fsm.SwitchState(fsm.getHuntedState);
    }
}
