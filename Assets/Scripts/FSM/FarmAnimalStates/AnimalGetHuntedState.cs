using UnityEngine;

public class AnimalGetHuntedState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        fsm.OnWalk.Invoke();
        fsm.Agent.SetDestination(fsm.GetRandomPos(AnimalManager.Instance.Meadow.position, 5f));
    }

    public override void UpdateState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.GetHunted)
        {
            if (fsm.Agent.hasPath && fsm.Agent.remainingDistance <= 3.60f)
            {
                fsm.Agent.radius = 0.5f;
                fsm.gameObject.GetComponent<Collider>().enabled = false;    // !!!!!
                fsm.Agent.SetDestination(fsm.transform.position);
                fsm.OnIdle.Invoke();
            }
        }
        else
            ExitState(fsm);
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {

    }

    public override void ExitState(AnimalBase fsm)
    {
        AnimalManager.Instance.Meadow = AnimalManager.Instance.Home;
        HelperController.Instance.ShowHelper(HelpType.BarnPanel);

        if (fsm.executingState == ExecutingAnimalState.Dead)
            fsm.SwitchState(fsm.deadState);
        else if (fsm.executingState == ExecutingAnimalState.GoToMeadow)
            fsm.SwitchState(fsm.goToMeadowState);
    }
}
