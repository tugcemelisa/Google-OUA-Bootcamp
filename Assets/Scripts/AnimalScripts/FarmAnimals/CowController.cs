using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum ExecutingCowState
{
    MoveAround,
    Graze,
    Flee,
    FollowHerd,
    Rest,
    GetMilked,
    DoNothing
}

public class CowController : AnimalBase, IFarmAnimal
{
    #region Actions
    [HideInInspector] public Action OnGraze;
    [HideInInspector] public Action OnGrazeFinish;
    [HideInInspector] public Action OnFlee;
    #endregion

    #region FSM
    public ExecutingCowState executingState;
    public CowStates currentState;
    [HideInInspector] public CowMoveAroundState moveAroundState = new();
    [HideInInspector] public CowGrazeState grazeState = new();
    [HideInInspector] public CowFleeState fleeState = new();
    [HideInInspector] public CowFollowHerdState followHerdState = new();
    [HideInInspector] public CowRestState restState = new();
    [HideInInspector] public CowGetMilkedState getMilkedState = new();
    #endregion

    private Vector3 randomPoint;
    public float detectionRadius = 4f;
    public float moveRadius = 10f;

    public float grazeTime = 6f;
    [HideInInspector] public float _grazeTimer;

    private NavMeshHit hit;

    public override void Start()
    {
        base.Start();
        executingState = ExecutingCowState.Graze;
        currentState = grazeState;
        currentState.EnterState(this);

        //_grazeTimer = grazeTime;
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void StartMove()
    {
        if (_playerTransform != null)
        {
            OnWalk.Invoke();
            Debug.Log("walk in startmove");
            Agent.SetDestination(GetRandomPos(_playerTransform.position, 15f));
        }

    }
    public void CheckIfArrived()
    {
        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            //Debug.Log(name + " arrived");
            executingState = ExecutingCowState.Graze;
        }
    }

    float distanceToPlayer;
    Vector3 fleeDirection;
    Vector3 fleePosition;
    public void StartFlee()
    {
        fleeDirection = (transform.position - _playerTransform.position).normalized;
        fleePosition = transform.position + fleeDirection * moveRadius;

        if (NavMesh.SamplePosition(fleePosition, out hit, 1f, NavMesh.AllAreas))
        {
            if (IsValidDestination(hit.position))
            {
                Agent.SetDestination(hit.position);
            }
            else
            {
                FindAlternativeDestination();
            }
        }
        else
        {
            FindAlternativeDestination();
        }
    }

    private void FindAlternativeDestination()
    {
        for (int i = 0; i < 5; i++)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * 15f;
            randomDirection += _playerTransform.position;
            NavMeshHit navHit;
            if (NavMesh.SamplePosition(randomDirection, out navHit, 15f, NavMesh.AllAreas))
            {
                if (IsValidDestination(navHit.position))
                {
                    Agent.SetDestination(navHit.position);
                    return;
                }
            }
        }
        Debug.Log("Suitable destination not found. Stopping agent.");
        Agent.SetDestination(transform.position);
    }
    private bool IsValidDestination(Vector3 position)
    {
        NavMeshPath path = new NavMeshPath();
        if (Agent.CalculatePath(position, path))
        {
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                return true;
            }
        }
        return false;
    }


    public void Flee()
    {
        distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < detectionRadius)
        {
            IPlayer = _playerTransform.GetComponent<IPlayer>();
            if (IPlayer != null)
            {
                IPlayer.ScareAnimal(Agent);
            }

            StartFlee();
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
            //_grazeTimer = grazeTime;
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

        if (_herdHeartbeat <= 0f)
        {
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
    }
    public void FollowHerd()
    {
        herdDirection /= neighborCount;
        Vector3 herdPosition = transform.position + herdDirection.normalized * moveRadius;
        Agent.SetDestination(herdPosition);
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
    Vector3 GetRandomPositionInBarn(Vector3 barnPosition, Vector3 barnScale)
    {
        //Bounds barnBounds = new Bounds(barnPosition, barnScale);

        Vector3 barnMin = barnPosition - (barnScale / 2);
        Vector3 barnMax = barnPosition + (barnScale / 2);

        Vector3 randomPosition = new Vector3(
            UnityEngine.Random.Range(barnMin.x, barnMax.x),
            barnPosition.y, 
            UnityEngine.Random.Range(barnMin.z, barnMax.z)
        );

        return randomPosition;
    }

    public void GetMilked()
    {
        distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < 1.5f)
        {
            IPlayer = _playerTransform.GetComponent<IPlayer>();
            if (IPlayer != null)
            {
                IPlayer.Milk(transform);
                IPlayer.StartMilk();
            }
        }
    }

    public void StandIdle()
    {
        executingState = ExecutingCowState.DoNothing;
    }

    private Vector3 GetRandomPos(Vector3 center, float range)
    {
        randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);

        return hit.position;
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
