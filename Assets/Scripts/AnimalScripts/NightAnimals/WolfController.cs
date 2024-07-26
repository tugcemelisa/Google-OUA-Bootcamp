using UnityEngine;
using Unity;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;

public class WolfController : MonoBehaviour
{
    WolfStates executingState;
    /*[SerializeField]*/public NavMeshAgent agent;
    /*[SerializeField]*/ public Transform target;
    [SerializeField] Animator animator;

    [SerializeField] float attackRange = 1.5f;

    [SerializeField] float attackDelay = 2f;

    [SerializeField] float attackDamage = 4f;

    bool isMoving = false;
    bool isAttacking = false;

    bool isTargetAttackable = false;

    [Header("Fear")]
    [SerializeField] float maxFear = 10;
    [SerializeField] private Image fearUI;
    float fear = 0;

    private void OnEnable()
    {
        WolfManager.OnWolfsAppear += Startt;
    }
    private void OnDisable()
    {
        WolfManager.OnWolfsAppear -= Startt;
    }

    private void Start()
    {
        executingState = WolfStates.Flee;
    }
    private void Startt()
    {
        executingState = WolfStates.TargetDetected;
    }
    private void Update()
    {
        Hunt();
    }

    private void Hunt()
    {
        if (hasTarget())
        {
            if (isTargetInAttackRange())
            {
                agent.isStopped = true;
                isMoving = false;
                //Debug.Log(isTargetAttackable + " " + isAttacking);
                if (isTargetAttackable && !isAttacking)
                {
                    Attack();
                    Debug.Log("target attacable");
                }
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
        //Debug.Log((target.position - transform.position).magnitude);
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
        Debug.Log(name + "wolf attacking cow");
        GiveDamage();
        isAttacking = false;
    }

    void MoveToTarget()
    {
        if (!isMoving)
        {
            animator.SetTrigger("Run");
            isMoving = true;
            agent.isStopped = false;
            return;
        }
  
        agent.SetDestination(target.position);
        //Debug.Log(name + " MoveToTarget");
    }


    public void GiveDamage()
    {
        var cow = target.GetComponent<AnimalBase>();
        if (cow) cow.TakeDamage(attackDamage);
        Debug.Log(name + "wolf damage cow..");
    }

    public void AssignNewTarget(Transform target, bool isDamageable)
    {
        this.target = target;
        this.isTargetAttackable = isDamageable;
        Debug.Log(target.name + " is " + isTargetAttackable);
    }

    public void StartCircleRun(Transform target)
    {
        //Debug.Log(name + " StartCircleRun");
        AssignNewTarget(target, false);
        isMoving = false;
        MoveToTarget();
        StartCoroutine(RunToTheCircle());
    }

    IEnumerator RunToTheCircle()
    {
        //Debug.Log(name + " RunToTheCircle");
        while (Vector3.Distance(transform.position, target.position) > .5f)    // 0.15f    !!!!!
        {
            //Debug.Log(Vector3.Distance(transform.position, target.position));
            yield return new WaitForFixedUpdate();
        }
        agent.isStopped = true;
        WolfManager.Instance.AddWolfToTheCircle(this, target);
    }

    public void AddFear(float amount)
    {
        //Debug.Log(name + " scares");
        fear += amount;
        fearUI.fillAmount = fear / maxFear;
        CheckFear();
    }

    public void CheckFear()
    {
        if (fear >= maxFear)
        {
            //Debug.Log(name + " is done");
            // TO UPDATE
            WolfManager.Instance.RunAway(this);
        }
    }
}

enum WolfStates
{
    TargetDetected,
    MoveToTarget,
    AttackToTarget,
    Flee,
    Hunt
}