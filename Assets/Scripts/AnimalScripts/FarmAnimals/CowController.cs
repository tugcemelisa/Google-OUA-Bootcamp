using UnityEngine;
using UnityEngine.AI;

enum CowStates
{
    MoveAround,
    Graze,
    Flee
}

public class CowController : MonoBehaviour
{
    CowStates executingState;
    [SerializeField] private Transform _player;
    private Vector3 randomPoint;
    public float detectionRadius = 5f;
    public float moveRadius = 10f;

    public float grazeTime = 3f;
    private float _grazeTimer;

    private NavMeshAgent Agent;
    private NavMeshHit hit;

    private void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        executingState = CowStates.Graze;
        _grazeTimer = grazeTime;
    }

    private void Update()
    {
        switch (executingState)
        {
            case CowStates.MoveAround:
                MoveAround();
                break;
            case CowStates.Graze:
                Graze();
                break;
            case CowStates.Flee: 
                Flee(); 
                break;
        }
    }

    private bool _isMoving;
    private void MoveAround()
    {
        if (_player != null)
        {
            if (!_isMoving)
            {
                Agent.SetDestination(GetRandomPos(_player.position, 50f));
                _isMoving = true;
            }
                
            if (!Agent.hasPath || Agent.velocity.sqrMagnitude == 0f)
            {
                _isMoving = false;
                executingState = CowStates.Graze;
            }    
        }

        CheckDistanceToPlayer();
    }

    float distanceToPlayer;
    private void Flee()
    {
        Vector3 fleeDirection = (transform.position - _player.position).normalized;
        Vector3 fleePosition = transform.position + fleeDirection * moveRadius;
        Agent.SetDestination(fleePosition);

        executingState = CowStates.Graze;
    }
    private void CheckDistanceToPlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        if (distanceToPlayer < detectionRadius)
        {
            executingState = CowStates.Flee;
        }
    }


    private Vector3 GetRandomPos(Vector3 center, float range)
    {
        randomPoint = center + Random.insideUnitSphere * range;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);

        return hit.position;
    }

    private void Graze()
    {
        _grazeTimer -= Time.deltaTime;
        if (_grazeTimer <= 0f)
        {
            executingState = CowStates.MoveAround;
            _grazeTimer = grazeTime;
        }

        CheckDistanceToPlayer();
    }
}
