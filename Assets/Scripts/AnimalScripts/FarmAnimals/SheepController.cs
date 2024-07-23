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

    private Vector3 randomPoint;
    public float detectionRadius = 4f;
    public float moveRadius = 10f;

    public float grazeTime = 6f;
    [HideInInspector] public float _grazeTimer;

    private NavMeshHit hit;

    public override void Start()
    {
        base.Start();
        executingState = ExecutingSheepState.MoveAround;
        currentState = moveAroundState;
        currentState.EnterState(this);
        previousPosition = transform.position;
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        currentState.Interact(this, interactKey);
    }

    private Vector3 previousPosition;
    private void HandleTurningAnimation()
    {
        Vector3 direction = transform.position - previousPosition;
        if (direction != Vector3.zero)
        {
            float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
            if (angle > 10f)
            {
                OnRightTurn.Invoke();
            }
            else if (angle < -10f)
            {
                OnLeftTurn.Invoke();
            }
        }
    }

    public void StartMove()
    {
        if (_playerTransform != null)
        {
            OnWalk.Invoke();
            Agent.SetDestination(GetRandomPos(_playerTransform.position, 15f));
        }

    }
    public void CheckIfArrived()
    {
        if (Agent.remainingDistance <= Agent.stoppingDistance)
        {
            executingState = ExecutingSheepState.Graze;
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

    private Vector3 GetRandomPos(Vector3 center, float range)
    {
        randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);

        return hit.position;
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
