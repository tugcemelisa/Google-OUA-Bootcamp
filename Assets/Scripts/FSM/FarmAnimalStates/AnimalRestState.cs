using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class AnimalRestState : AnimalStates
{
    public override void EnterState(AnimalBase fsm)
    {
        //Debug.Log("REST " + fsm.gameObject.name);

        lastPosition = fsm.Agent.transform.position;
        stuckCheckIntervalTimer = stuckCheckInterval;
        stuckTime = 0.0f;

        //InteractableUIElement activeUIElement;
        //if(fsm.InteractableUIElements.Any(x => x.enabled))
        //{
        //    activeUIElement = fsm.InteractableUIElements.Where(x => x.enabled);
        //}
    }

    public override void UpdateState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.Rest)
        {
            stuckCheckIntervalTimer -= Time.deltaTime;
            if(stuckCheckIntervalTimer <= 0.0f)
            {
                CheckIfStuck(fsm.Agent);
                stuckCheckIntervalTimer = stuckCheckInterval;
            }

            if (fsm.Agent.remainingDistance <= /*fsm.Agent.stoppingDistance*/ 3.60f)
            {
                fsm.Agent.SetDestination(fsm.transform.position);
                fsm.OnIdle.Invoke();
                fsm.executingState = ExecutingAnimalState.GetUsed;
            } 
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(AnimalBase fsm)
    {
        if (fsm.executingState == ExecutingAnimalState.GetUsed)
            fsm.SwitchState(fsm.getMilkedState);
    }


    private float maxTimeToReachDestination = 2.0f;
    private float stuckThreshold = 0.1f; // Hareket etmeme eþiði
    private float stuckCheckInterval = 1.0f; // Hareket kontrol aralýðý
    private float stuckCheckIntervalTimer;

    private Vector3 lastPosition;
    private float stuckTime;

    void CheckIfStuck(NavMeshAgent agent)
    {
        if (Vector3.Distance(agent.transform.position, lastPosition) < stuckThreshold)
        {
            stuckTime += stuckCheckInterval;

            if (stuckTime >= maxTimeToReachDestination)
            {
                Debug.Log(agent.gameObject.name + " stuck, stopping.");
                //agent.isStopped = true;
                agent.SetDestination(agent.transform.position);
                agent.isStopped = true;
            }
        }
        else
        {
            stuckTime = 0.0f;
        }

        lastPosition = agent.transform.position;
    }

    public override void Interact(AnimalBase fsm, KeyCode interactKey)
    {
        
    }
}
