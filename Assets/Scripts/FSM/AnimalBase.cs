using System;
using System.Collections;
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
    [HideInInspector] public Action OnGetScared;
    [HideInInspector] public Action OnHurt;
    [HideInInspector] public Action OnDie;
    #endregion

    public Transform meadow;

    protected Transform _playerTransform;
    protected IPlayer IPlayer;

    [HideInInspector] public NavMeshAgent Agent;

    [HideInInspector] public float acceleration;
    [HideInInspector] public float speed;

    private Vector3 randomPoint;
    public float detectionRadius = 4f;
    public float moveRadius = 15f;

    public float grazeTime = 6f;
    [HideInInspector] public float _grazeTimer;

    private NavMeshHit hit;

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

    public void Flee()
    {
        distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < detectionRadius)
        {
            StartFlee();
        }
    }

    public void StartMove()
    {
        if (_playerTransform != null)
        {
            OnWalk.Invoke();
            Agent.SetDestination(GetRandomPos(_playerTransform.position, 7f));  // 15f
        }

    }

    protected float distanceToPlayer;
    Vector3 fleeDirection;
    Vector3 fleePosition;

    public void StartFlee()
    {
        if (meadow != null)
        {
            AvoidOtherAnimals();
        }
        else
        {
            fleeDirection = (transform.position - _playerTransform.position).normalized;
            fleePosition = transform.position + fleeDirection * moveRadius;
            NavMesh.SamplePosition(fleePosition, out hit, 5f, NavMesh.AllAreas);
            Agent.SetDestination(hit.position);
        }
    }

    public abstract void AvoidOtherAnimals();

    public void GetScared()
    {
        IPlayer = _playerTransform.GetComponent<IPlayer>();
        if (IPlayer != null)
        {
            IPlayer.ScareAnimal(Agent);
            Agent.acceleration = 16;
            Agent.speed = 3;
            OnGetScared.Invoke();
            StartCoroutine(ResetAnimalAfterTime(5f));
        }
    }
    private IEnumerator ResetAnimalAfterTime(float delay)
    {
        yield return new WaitForSeconds(delay);
        Agent.acceleration = acceleration;
        Agent.speed = speed;
        OnWalk.Invoke();
    }

    public Vector3 GetFleeDirection()
    {
        float distanceToShepherd = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToShepherd < detectionRadius)
        {
            return (transform.position - _playerTransform.position).normalized;
        }
        return Vector3.zero;
    }

    public void StartMoveToMeadow()
    {
        RejoinHerd();
    }

    public abstract void RejoinHerd();

    public Vector3 GetRandomPos(Vector3 center, float range)
    {
        randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);

        return hit.position;
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

    public abstract void StartStraggle();
    public abstract void StartDanger();

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
        OnDie.Invoke();
        isAlive = false;

        hitPointUI.fillAmount = 0;
    }
}
