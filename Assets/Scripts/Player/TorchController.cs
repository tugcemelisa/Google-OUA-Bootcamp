using System;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float attackCooldown = 1f;
    float cooldown = 0;
    bool canAttack = true;

    [SerializeField] float fearPerAttack = 5f;

    [SerializeField] Transform torchHead;
    [SerializeField] float fearAttackRadius = 3f;
    [SerializeField] LayerMask layerToGiveFear;

    [SerializeField] float pettingAnimationTime = 3f;
    [SerializeField] float gatheringAnimationTime = 2.5f;

    private void OnEnable()
    {
        GameModeManager.OnNightStart += () => torchHead.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        GameModeManager.OnNightStart -= () => torchHead.gameObject.SetActive(true);
    }

    private void Start()
    {
        torchHead.gameObject.SetActive(false);
    }

    public void ControlAttack()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            cooldown = 0;
            canAttack = true;
        }

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            canAttack = false;
            cooldown = attackCooldown;
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StarterAssets.InputController.Instance.DisableInputs();
            animator.SetTrigger("Gather");
        }
    }

    public void Pet(Transform dog)
    {
        StarterAssets.InputController.Instance.DisableInputs();
        animator.SetTrigger("Petting");
        transform.LookAt(dog);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        GiveFear();
    }

    private void GiveFear()
    {
        var rayCastHits = Physics.SphereCastAll(torchHead.position, fearAttackRadius, -Vector3.up, layerToGiveFear);

        foreach (var hit in rayCastHits)
        {
            var wolf = hit.collider.GetComponent<WolfController>();
            if (wolf)
            {
                wolf.AddFear(fearPerAttack);
                Debug.Log(name + " scares " + wolf.name + " TORCHCONTROLLER");
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(torchHead.position, fearAttackRadius);
    }
}
