using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum ExecutingCowState
{
    WaitInBarn,
    MoveAround,
    Graze,
    Flee,
    FollowHerd,
    GoToMeadow,
    GetHunted,
    Rest,
    GetMilked,
    DoNothing
}

public class CowController : AnimalBase
{
    #region FSM
    public ExecutingCowState executingState;
    public CowStates currentState;
    [HideInInspector] public CowWaitInBarnState waitInBarnState = new();
    [HideInInspector] public CowMoveAroundState moveAroundState = new();
    [HideInInspector] public CowGrazeState grazeState = new();
    [HideInInspector] public CowFleeState fleeState = new();
    [HideInInspector] public CowFollowHerdState followHerdState = new();
    [HideInInspector] public CowGoToMeadowState goToMeadowState = new();
    [HideInInspector] public CowGetHuntedState getHuntedState = new();
    [HideInInspector] public CowRestState restState = new();
    [HideInInspector] public CowGetMilkedState getMilkedState = new();
    [HideInInspector] public CowDoNothingState doNothingState = new();
    #endregion


    public override void Start()
    {
        base.Start();
        executingState = ExecutingCowState.WaitInBarn;
        currentState = waitInBarnState;
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

    public override void StartDanger()
    {
        executingState = ExecutingCowState.GetHunted;
    }

    public void CheckIfArrived()
    {
        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            executingState = ExecutingCowState.Graze;
        }
    }

    public override void StartStraggle()
    {
        if(executingState == ExecutingCowState.GoToMeadow)
            executingState = ExecutingCowState.Flee;  
    }

    public  override void AvoidOtherAnimals()
    {
        Collider[] nearbyCows = Physics.OverlapSphere(transform.position, cowDetectionRadius);
        foreach (Collider cow in nearbyCows)
        {
            if (cow.gameObject != this.gameObject && cow.CompareTag("Cow"))
            {
                Vector3 avoidDirection = (transform.position - cow.transform.position).normalized;
                Vector3 avoidPosition = transform.position + avoidDirection * moveRadius;
                Agent.SetDestination(avoidPosition);
            }
        }
    }

    public void CheckDistanceToPlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < detectionRadius)
        {
            executingState = ExecutingCowState.Flee;
        }
    }

    public void Graze()
    {
        _grazeTimer -= Time.deltaTime;
        if (_grazeTimer <= 0f)
        {
            executingState = ExecutingCowState.MoveAround;
        }
    }

    public float cowDetectionRadius = 4f;
    Vector3 herdDirection;
    int neighborCount;

    [HideInInspector] public float _herdHeartbeat;
    [HideInInspector] public float _maxDuration = 1f;
    public void FindNearestHerd()
    {
        _herdHeartbeat -= Time.deltaTime;

        herdDirection = Vector3.zero;
        neighborCount = 0;

        Collider[] nearbyCows = Physics.OverlapSphere(transform.position, cowDetectionRadius);
        foreach (Collider cow in nearbyCows)
        {
            if (cow.gameObject != this.gameObject && cow.CompareTag("Cow"))
            {
                herdDirection += cow.transform.forward;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            executingState = ExecutingCowState.FollowHerd;
        }

        _herdHeartbeat = _maxDuration;
    }

    public void FollowHerd()
    {
        Vector3 finalDirection = Vector3.zero;
        Vector3 fleeDirection = GetFleeDirection();
        herdDirection /= neighborCount;
        if (fleeDirection != Vector3.zero)
        {
            finalDirection = fleeDirection + herdDirection;
        }
        else if (herdDirection != Vector3.zero)
        {
            finalDirection = herdDirection;
        }
        Vector3 herdPosition = transform.position + finalDirection.normalized * moveRadius;
        Agent.SetDestination(herdPosition);

        Collider[] nearbyCows = Physics.OverlapSphere(transform.position, cowDetectionRadius);
        if (nearbyCows.Length !>  0)
        {
            executingState = ExecutingCowState.Graze;
        }
    }

    public override void RejoinHerd()
    {
        if(meadow != null)
        {
            executingState = ExecutingCowState.GoToMeadow;
        }
    }

    public void SettleInBarn()
    {
        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            if (hit.collider.CompareTag("Barn"))
            {
                Agent.SetDestination(GetRandomPos(hit.transform.position + Vector3.back*2, 1.55f));
                executingState = ExecutingCowState.Rest;
            }
        }
    }

    public void GetMilked(KeyCode interactKey)
    {
        if ((int)interactKey == (int)InteractKeys.InteractAnimals)
        {
            IPlayer = _playerTransform.GetComponentInParent<IPlayer>();
            if (IPlayer != null)
            {
                IPlayer.Milk(transform);
            }
        }
    }

    public override void StandIdle(float duration)
    {
        executingState = ExecutingCowState.DoNothing;

        Invoke("ChangeUIElement", duration);
    }

    public void SwitchState(CowStates nextState)
    {
        if (currentState != nextState)
        {
            currentState = nextState;
            currentState.EnterState(this);
        }
    }
}
