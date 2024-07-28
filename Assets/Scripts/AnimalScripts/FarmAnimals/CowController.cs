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
    [HideInInspector] public Action OnGetScared;
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

    private Vector3 randomPoint;
    public float detectionRadius = 4f;
    public float moveRadius = 15f;

    public float grazeTime = 6f;
    [HideInInspector] public float _grazeTimer;

    private NavMeshHit hit;

    public override void StartDanger()
    {
        executingState = ExecutingCowState.GetHunted;
    }

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

    public void StartMove()
    {
        if (_playerTransform != null)
        {
            OnWalk.Invoke();
            Agent.SetDestination(GetRandomPos(_playerTransform.position, 7f));
        }

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
    public void AvoidOtherCows()
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

    float distanceToPlayer;
    Vector3 fleeDirection;
    Vector3 fleePosition;
    public void StartFlee()
    {
        if(meadow != null)
        {
            AvoidOtherCows();
        }
        else
        {
            fleeDirection = (transform.position - _playerTransform.position).normalized;
            fleePosition = transform.position + fleeDirection * moveRadius;
            NavMesh.SamplePosition(fleePosition, out hit, 5f, NavMesh.AllAreas);
            Agent.SetDestination(hit.position);
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
            StartFlee();
        }
    }

    public void GetScared()
    {
        IPlayer = _playerTransform.GetComponent<IPlayer>();
        if (IPlayer != null)
        {
            IPlayer.ScareAnimal(Agent);
            Agent.acceleration = 16;
            Agent.speed = 3;
            OnGetScared.Invoke();
            StartCoroutine(ResetAnimalAfterTime(5f));
        }
    }
    private IEnumerator ResetAnimalAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        Agent.acceleration = acceleration;
        Agent.speed = speed;
        OnWalk.Invoke();
    }

    public Vector3 GetFleeDirection()
    {
        float distanceToShepherd = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToShepherd < detectionRadius)
        {
            return (transform.position - _playerTransform.position).normalized;
        }
        return Vector3.zero;
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

        //if (_herdHeartbeat <= 0f)
        //{
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
        //}  
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

    public override void StartMoveToMeadow()
    {
        RejoinHerd();
    }

    public void RejoinHerd()
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

    public Vector3 GetRandomPos(Vector3 center, float range)
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
