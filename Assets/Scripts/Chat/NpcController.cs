using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum NpcState
{
    WalkAround,
    Wait
}
public class NpcController : MonoBehaviour, INpc
{
    PlayerInteract playerInteract;
    public NpcState executingState;
    private NavMeshAgent Agent;
    private NavMeshHit hit;
    Animator animator;

    public Transform _marketplace;
    private Transform _player;
    private Vector3 randomPoint;

    public virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        playerInteract = _player.GetComponent<PlayerInteract>();
        executingState = NpcState.WalkAround;
    }

    public void Update()
    {
        switch (executingState)
        {
            case NpcState.WalkAround:
                MoveAround();
                break;
            case NpcState.Wait:
                Wait();
                break;
        }
    }

    private void MoveAround()
    {
        if (!Agent.hasPath)
        {
            if(_marketplace != null)
            {
                Agent.SetDestination(GetRandomPos(_marketplace.position, 15f));
                animator.SetTrigger("Walk");
            }
        }
    }

    public void Talk()
    {
        if(executingState != NpcState.Wait)
        {
            RotateToPlayer();
            executingState = NpcState.Wait;
            animator.SetTrigger("Idle");
        }
    }

    private void Wait()
    {
        Agent.SetDestination(transform.position);
        
        if (playerInteract.GetInteractable() == null)
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
        if (_player != null)
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            Vector3 targetEulerAngles = Quaternion.LookRotation(direction).eulerAngles;
            transform.DORotate(targetEulerAngles, 2);
        }  
    }
}
