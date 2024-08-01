using UnityEngine;
using Unity;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class WolfController : Interactable
{
    [SerializeField] private NavMeshAgent agent;
    private Transform target;
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
    bool isFeared = false;

    WolfStates state = WolfStates.None;

    public NavMeshAgent Agent { get => agent; }

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
                Agent.isStopped = true;
                isMoving = false;

                if (isTargetAttackable && !isAttacking)
                {
                    Attack();
                    Debug.Log("target attackable");
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
        return HorizontalDistance(target.position, transform.position) < attackRange;
    }

    void Attack()
    {
        isAttacking = true;
        RotateToPrey();     // !!!!!!!
        animator.SetTrigger("Attack");
        Debug.Log(name + " " + animator.ToString());
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
            Agent.isStopped = false;
            return;
        }

        Agent.SetDestination(target.position);
    }

    public void RotateToPrey()
    {
        Vector3 direction = (target.position + new Vector3(-0.5f, 0, 0) - transform.position).normalized;
        float singleStep = 2 * Time.deltaTime;
        Vector3 targetEulerAngles = Quaternion.LookRotation(direction).eulerAngles;
        transform.DORotate(targetEulerAngles, 2);
    }


    public void GiveDamage()
    {
        var animal = target.GetComponent<AnimalBase>();
        if (animal) animal.TakeDamage(attackDamage);
        Debug.Log(name + "wolf damage animal..");
    }

    public void AssignNewTarget(Transform target, bool isDamageable)
    {
        this.target = target;
        this.isTargetAttackable = isDamageable;
        //if(isDamageable)
        //{
        //    RotateToPrey();
        //}
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
        while (HorizontalDistance(transform.position, target.position) > Agent.stoppingDistance)    // 0.15f    !!!!!
        {
            yield return new WaitForFixedUpdate();
        }
        Agent.isStopped = true;
        //
        animator.SetTrigger("Idle");
        //
        WolfManager.Instance.AddWolfToTheCircle(this, target);
    }

    float HorizontalDistance(Vector3 start, Vector3 end)
    {
        start.y = end.y;
        return Vector3.Distance(start, end);
    }

    public void AddFear(float amount)
    {
        fear += amount;
        fearUI.fillAmount = fear / maxFear;
        CheckFear();
    }

    public void CheckFear()
    {
        if (isFeared) return;
        if (fear >= maxFear)
        {
            isFeared = true;
            // TO UPDATE
            WolfManager.Instance.RunAway(this);

            //Sound
            SoundManager.Instance.PlaySound(VoiceType.WolfScaried, transform, transform.position);
        }
    }

    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        //if ((int)interactKey == (int)InteractKeys.InteractAnimals)
        //{

        //}
    }

    public void SetState(WolfStates state)
    {
        this.state = state;

        switch (state)
        {
            case WolfStates.RunInsideCircle:
                animator.SetTrigger("Run");
                break;
            default:
                break;
        }
    }
}

public enum WolfStates
{
    None,
    TargetDetected,
    MoveToTarget,
    AttackToTarget,
    Flee,
    RunInsideCircle,
}