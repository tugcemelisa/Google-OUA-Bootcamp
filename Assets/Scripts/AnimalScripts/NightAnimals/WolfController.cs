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

    bool isTargetAttackable = false;

    private void Update()
    {
        if (hasTarget())
        {
            if (isTargetInAttackRange())
            {
                agent.isStopped = true;
                isMoving = false;

                if (isTargetAttackable && !isAttacking)
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
        else
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

    public void AssignNewTarget(Transform target, bool isDamageable)
    {
        this.target = target;
        this.isTargetAttackable = isDamageable;
    }

    public void StartCircleRun(Transform target)
    {
        AssignNewTarget(target, false);
        isMoving = false;
        MoveToTarget();
        StartCoroutine(RunToTheCircle());
    }

    IEnumerator RunToTheCircle()
    {
        while(Vector3.Distance(transform.position, target.position) > .15f)
        {
            yield return new WaitForFixedUpdate();
        }
        agent.isStopped = true;
        WolfManager.Instance.AddWolfToTheCircle(this,target);
    }
}

enum WolfStates
{
    TargetDetected,
    MoveToTarget,
    AttackToTarget,
    Flee
}