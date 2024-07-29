using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum ExecutingSheepState
{
    MoveAround,
    Graze,
    Flee,
    FollowHerd,
    Rest,
    GetSheared,
    DoNothing
}

public class SheepController : AnimalBase
{
    [HideInInspector] public Action OnRun;
    [HideInInspector] public Action OnSit;
    [HideInInspector] public Action OnRightTurn;
    [HideInInspector] public Action OnLeftTurn;

    #region FSM
    public ExecutingSheepState executingState;
    public SheepStates currentState;
    [HideInInspector] public SheepMoveAroundState moveAroundState = new();
    [HideInInspector] public SheepGrazeState grazeState = new();
    [HideInInspector] public SheepFleeState fleeState = new();
    [HideInInspector] public SheepFollowHerdState followHerdState = new();
    [HideInInspector] public SheepRestState restState = new();
    [HideInInspector] public SheepGetShearedState getShearedState = new();
    [HideInInspector] public SheepDoNothingState doNothingState = new();
    #endregion


    public override void Start()
    {
        base.Start();
        executingState = ExecutingSheepState.MoveAround;
        currentState = moveAroundState;
        currentState.EnterState(this);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        currentState.Interact(this, interactKey);
    }

    public void CheckIfArrived()
    {
        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            executingState = ExecutingSheepState.Graze;
        }
    }

    public void CheckDistanceToPlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < detectionRadius)
        {
            executingState = ExecutingSheepState.Flee;
        }
    }

    public void Graze()
    {
        _grazeTimer -= Time.deltaTime;
        if (_grazeTimer <= 0f)
        {
            executingState = ExecutingSheepState.MoveAround;
        }
    }

    public float sheepDetectionRadius = 2f;
    Vector3 herdDirection;
    int neighborCount;

    [HideInInspector] public float _herdHeartbeat;
    [HideInInspector] public float _maxDuration = 0.4f;
    public void FindNearestHerd()
    {
        _herdHeartbeat -= Time.deltaTime;

        if (_herdHeartbeat <= 0f)
        {
            herdDirection = Vector3.zero;
            neighborCount = 0;

            Collider[] nearbySheeps = Physics.OverlapSphere(transform.position, sheepDetectionRadius);
            foreach (Collider sheep in nearbySheeps)
            {
                if (sheep.gameObject != this.gameObject && sheep.CompareTag("Sheep"))
                {
                    herdDirection += sheep.transform.forward;
                    neighborCount++;
                }
            }

            if (neighborCount > 0)
            {
                executingState = ExecutingSheepState.FollowHerd;
            }

            _herdHeartbeat = _maxDuration;
        }
    }
    public void FollowHerd()
    {
        herdDirection /= neighborCount;
        Vector3 herdPosition = transform.position + herdDirection.normalized * moveRadius;
        Agent.SetDestination(herdPosition);
    }

    public override void AvoidOtherAnimals()
    {
        Collider[] nearbyCows = Physics.OverlapSphere(transform.position, sheepDetectionRadius);
        foreach (Collider cow in nearbyCows)
        {
            if (cow.gameObject != this.gameObject && cow.CompareTag("Sheep"))
            {
                Vector3 avoidDirection = (transform.position - cow.transform.position).normalized;
                Vector3 avoidPosition = transform.position + avoidDirection * moveRadius;
                Agent.SetDestination(avoidPosition);
            }
        }
    }

    public override void StartStraggle()
    {
        executingState = ExecutingSheepState.Flee;
    }

    public override void RejoinHerd()
    {
        if (meadow != null)
        {
            //executingState = ExecutingSheepState.GoToMeadow;
        }
    }
    public override void StartDanger()
    {
        //executingState = ExecutingSheepState.GetHunted;
    }

    public void SettleInBarn()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            if (hit.collider.CompareTag("Barn"))
            {
                Agent.SetDestination(GetRandomPos(hit.transform.position + Vector3.back * 2, 1.55f));
                executingState = ExecutingSheepState.Rest;
            }
        }
    }

    public void GetSheared(KeyCode interactKey)
    {
        if ((int)interactKey == (int)InteractKeys.InteractAnimals)
        {
            IPlayer = _playerTransform.GetComponentInParent<IPlayer>();
            if (IPlayer != null)
            {
                IPlayer.Shear(transform);
            }
        }
    }

    public override void StandIdle(float duration)
    {
        executingState = ExecutingSheepState.DoNothing;

        Invoke("ChangeUIElement", duration);
    }

    public void SwitchState(SheepStates nextState)
    {
        if (currentState != nextState)
        {
            currentState = nextState;
            currentState.EnterState(this);
        }
    }
}
