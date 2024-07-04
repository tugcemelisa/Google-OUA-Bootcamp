using System;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float attackCooldown = 1f;
    float cooldown = 0;
    bool canAttack = true;


    private void Update()
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
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
    }
}
