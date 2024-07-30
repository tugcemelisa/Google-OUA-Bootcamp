using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum NpcState
{
    WalkAround,
    Interact,
    Wait
}

public class NpcController : MonoBehaviour, INpc
{
    public NpcState executingState;
    private NavMeshAgent Agent;
    private NavMeshHit hit;
    Animator animator;

    public Transform _barn;
    private Transform _player;
    private Vector3 randomPoint;

    public virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        _barn = GameObject.FindWithTag("Barn").GetComponent<Transform>();
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        executingState = NpcState.WalkAround;
    }

    public void Update()
    {
        switch (executingState)
        {
            case NpcState.WalkAround:
                MoveAround();
                break;
            //case NpcState.Interact:
            //    Talk();
            //    break;
            case NpcState.Wait:
                Wait();
                break;
        }
    }

    private void MoveAround()
    {
        if (!Agent.hasPath)
        {
            Agent.SetDestination(GetRandomPos(_player.position, 15f));
            animator.SetTrigger("Walk");
        }
    }

    public void Talk()
    {
        RotateToPlayer();
        executingState = NpcState.Wait;
        animator.SetTrigger("Talk");
    }

    private void Wait()
    {
        Agent.SetDestination(transform.position);
        var cols = Physics.OverlapSphere(transform.position, 3f);
        if (cols.Any(x => x != _player))
        {
            executingState = NpcState.WalkAround;
        }
    }

    public Vector3 GetRandomPos(Vector3 center, float range)
    {
        randomPoint = center + Random.insideUnitSphere * range;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);

        return hit.position;
    }

    public void RotateToPlayer()
    {
        Vector3 direction = (_barn.position + new Vector3(-0.5f, 0, 0) - transform.position).normalized;
        Vector3 targetEulerAngles = Quaternion.LookRotation(direction).eulerAngles;
        transform.DORotate(targetEulerAngles, 2);
    }
}
