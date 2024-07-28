using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public abstract class AnimalBase : Interactable, IFarmAnimal
{
    #region Actions
    [HideInInspector] public Action OnWalk;
    [HideInInspector] public Action OnIdle;
    [HideInInspector] public Action OnGraze;
    [HideInInspector] public Action OnGrazeFinish;
    [HideInInspector] public Action OnFlee;
    [HideInInspector] public Action OnHurt;
    [HideInInspector] public Action OnDie;
    #endregion

    //public AnimalStates currentState;
    public Transform meadow;

    protected Transform _playerTransform;
    protected IPlayer IPlayer;

    [HideInInspector] public NavMeshAgent Agent;

    [HideInInspector] public float acceleration;
    [HideInInspector] public float speed;

    [Header("Hit Point")]
    [SerializeField] private float hitPointMaximum;
    [SerializeField] private Image hitPointUI;

    bool isAlive = true;

    float hitPoint = 10;

    public void OnEnable()
    {
        PlayerSimulationController.OnHerdLeaveBarn += () => Invoke("StartMoveToMeadow", 3f);
        GameModeManager.OnNightStart += StartDanger;
    }
    public void OnDisable()
    {
        PlayerSimulationController.OnHerdLeaveBarn -= () => Invoke("StartMoveToMeadow", 3f);
        GameModeManager.OnNightStart -= StartDanger;
    }

    public virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        acceleration = Agent.acceleration;
        speed = Agent.speed;

        hitPoint = hitPointMaximum;
    }

    public abstract void StandIdle(float duration);

    public void ChangeUIElement()
    {
        foreach (var item in InteractableUIElements)
        {
            if (item.enabled)
            {
                item.Disable(true);

                PlayerInteractableUI.Instance.UpdateUIElements();
                break;
            }
        }
    }

    public abstract void StartMoveToMeadow();

    public abstract void StartStraggle();
    public abstract void StartDanger();

    public void TakeDamage(float amount)
    {
        if (!isAlive) return;
        hitPoint -= amount;
        CheckIsDead();
        Debug.Log(name + " taking damage");
    }

    private void CheckIsDead()
    {
        if (hitPoint <= 0)
        {
            Die();
        }
        else
        {
            //animator.SetTrigger("Hurt");
            OnHurt.Invoke();
            UpdateHitPointUI();
        }

    }

    private void UpdateHitPointUI()
    {
        hitPointUI.fillAmount = hitPoint / hitPointMaximum;
    }

    void Die()
    {
        //animator.SetTrigger("Dead");
        OnDie.Invoke();
        isAlive = false;

        hitPointUI.fillAmount = 0;

    }
}
