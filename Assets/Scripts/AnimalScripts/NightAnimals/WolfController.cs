using UnityEngine;
using Unity;
using UnityEngine.AI;
using System.Collections;

public class WolfController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;
    [SerializeField] Animator animator;

    [SerializeField] float attackRange = 1.5f;

    [SerializeField] float attackDelay = 2f;

    [SerializeField] float attackDamage = 4f;

    bool isMoving = false;
    bool isAttacking = false;

    private void Update()
    {
        if (hasTarget())
        {
            if (isTargetInAttackRange())
            {
                agent.isStopped = true;
                isMoving = false;

                if (!isAttacking)
                    Attack();
            }
            else
            {
                MoveToTarget();
            }
        }
    }

    bool hasTarget()
    {
        return target != null;
    }

    bool isTargetInAttackRange()
    {
        return (target.position - transform.position).magnitude < attackRange;
    }

    void Attack()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");
        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }

    void MoveToTarget()
    {
        if (isMoving)
        {
            return;
        }
        else if (!isTargetInAttackRange())
        {
            animator.SetTrigger("Run");
            agent.SetDestination(target.position);
            isMoving = true;
            agent.isStopped = false;
        }
    }


    public void GiveDamage()
    {
        target.GetComponent<NightCowController>().TakeDamage(attackDamage);
    }
}

enum WolfStates
{
    TargetDetected,
    MoveToTarget,
    AttackToTarget,
    Flee
}