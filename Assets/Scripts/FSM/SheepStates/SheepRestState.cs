using UnityEngine;
using UnityEngine.AI;

public class SheepRestState : SheepStates
{
    public override void EnterState(SheepController fsm)
    {
        Debug.Log("rest");
        lastPosition = fsm.Agent.transform.position;
        stuckCheckIntervalTimer = stuckCheckInterval;
        stuckTime = 0.0f;
    }

    public override void UpdateState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.Rest)
        {
            stuckCheckIntervalTimer -= Time.deltaTime;
            if (stuckCheckIntervalTimer <= 0.0f)
            {
                CheckIfStuck(fsm.Agent);
                stuckCheckIntervalTimer = stuckCheckInterval;
            }

            if (fsm.Agent.remainingDistance <= 3.60f)
            {
                fsm.Agent.SetDestination(fsm.transform.position);
                fsm.OnIdle.Invoke();
                fsm.executingState = ExecutingSheepState.GetSheared;
            }
        }
        else
            ExitState(fsm);
    }

    public override void ExitState(SheepController fsm)
    {
        if (fsm.executingState == ExecutingSheepState.GetSheared)
            fsm.SwitchState(fsm.getShearedState);
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
}
