using UnityEngine;
using Unity;
using UnityEngine.AI;
using System.Collections;
using System;
using UnityEngine.UI;

public class NightCowController : MonoBehaviour
{
    /*[SerializeField]*/ Animator animator;

    [Header("Hit Point")]
    [SerializeField] private float hitPointMaximum;
    [SerializeField] private Image hitPointUI;

    bool isAlive = true;

    float hitPoint = 10;

    private void Start()
    {
        animator = GetComponent<Animator>();
        hitPoint = hitPointMaximum;
    }

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;
        hitPoint -= amount;
        CheckIsDead();
    }

    private void CheckIsDead()
    {
        if (hitPoint <= 0)
        {
            Die();
        }
        else
        {
            animator.SetTrigger("Hurt");
            UpdateHitPointUI();
        }

    }

    private void UpdateHitPointUI()
    {
        hitPointUI.fillAmount = hitPoint / hitPointMaximum;
    }

    void Die()
    {
        animator.SetTrigger("Dead");
        isAlive = false;

        hitPointUI.fillAmount = 0;

    }
}
