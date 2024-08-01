using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum ExecutingAnimalState
{
    WaitInBarn,
    MoveAround,
    Graze,
    Flee,
    FollowHerd,
    GoToMeadow,
    GetHunted,
    Rest,
    GetUsed,
    DoNothing,
    Dead
}
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

    #region FSM
    public ExecutingAnimalState executingState;
    public AnimalStates currentState;
    [HideInInspector] public AnimalWaitInBarnState waitInBarnState = new();
    [HideInInspector] public AnimalMoveAroundState moveAroundState = new();
    [HideInInspector] public AnimalGrazeState grazeState = new();
    [HideInInspector] public AnimalFleeState fleeState = new();
    [HideInInspector] public AnimalFollowHerdState followHerdState = new();
    [HideInInspector] public AnimalGoToMeadowState goToMeadowState = new();
    [HideInInspector] public AnimalGetHuntedState getHuntedState = new();
    [HideInInspector] public AnimalRestState restState = new();
    [HideInInspector] public AnimalGetUsedState getUsedState = new();
    [HideInInspector] public AnimalDoNothingState doNothingState = new();
    [HideInInspector] public AnimalDeadState deadState = new();
    #endregion

    #region Variables..
    public Transform meadow;
    public Transform home;

    protected Transform _playerTransform;
    protected IPlayer IPlayer;

    [HideInInspector] public NavMeshAgent Agent;
    private NavMeshHit hit;
    [HideInInspector] public float acceleration;
    [HideInInspector] public float speed;

    private Vector3 randomPoint;
    public float detectionRadius = 4f;
    public float moveRadius = 15f;

    public float grazeTime = 6f;
    [HideInInspector] public float _grazeTimer;
    [HideInInspector] public float _herdHeartbeat;
    [HideInInspector] public float _maxDuration = 1f;


    [Header("Hit Point")]
    [SerializeField] private float hitPointMaximum;
    [SerializeField] private Image hitPointUI;

    bool isAlive = true;
    bool isBodyExist = true;

    float hitPoint = 10;
    #endregion

    #region Item

    [SerializeField] ItemMonoBehaviour item;
    [SerializeField] InteractableUIElement deadInteractable;

    public void SpawnItem()
    {
        if (item.ItemData.count < 0)
        {
            return;
        }
        item.transform.parent = null;
        item.transform.position += Vector3.up * 3;
        item.gameObject.SetActive(true);

        FadeOutTheBody();
    }

    void FadeOutTheBody()
    {
        if (!isBodyExist) { return; }

        isBodyExist = false;
        // Particle and Sound
        ParticleManager.Instance.PlayParticle(ParticleType.Disappear, null, transform.position);


        this.gameObject.SetActive(false);
    }
    void DecreaseItemCount()
    {
        if (item.ItemData.count < 0)
        {
            FadeOutTheBody();
            return;
        }

        item.ItemData.count -= 1;
    }

    public void ShowDeadInteractable()
    {
        foreach (var interactableUI in InteractableUIElements)
        {
            interactableUI.enabled = false;
        }
        deadInteractable.enabled = true;
    }

    #endregion

    public void OnEnable()
    {
        PlayerSimulationController.OnHerdLeaveBarn += () => Invoke("StartMoveToMeadow", 3f);
        GameModeManager.OnNightStart += StartDanger;
        WolfManager.OnHuntOver += ReturnVillage;
    }
    public void OnDisable()
    {
        PlayerSimulationController.OnHerdLeaveBarn -= () => Invoke("StartMoveToMeadow", 3f);
        GameModeManager.OnNightStart -= StartDanger;
        WolfManager.OnHuntOver -= ReturnVillage;   // moveAroundCenter......
    }

    public virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
        moveAroundCenter = _playerTransform;

        acceleration = Agent.acceleration;
        speed = Agent.speed;

        hitPoint = hitPointMaximum;

        executingState = ExecutingAnimalState.GetUsed;
        currentState = getUsedState;
        currentState.EnterState(this);
    }

    public void StartDanger()
    {
        executingState = ExecutingAnimalState.GetHunted;
    }

    private void ReturnVillage()
    {
        executingState = ExecutingAnimalState.GoToMeadow;
        gameObject.GetComponent<Collider>().enabled = true;
    }

    public void CheckIfArrived()
    {
        if (Agent.hasPath && Agent.remainingDistance <= Agent.stoppingDistance)
        {
            executingState = ExecutingAnimalState.Graze;
        }
    }

    public void StartStraggle()
    {
        if (executingState == ExecutingAnimalState.GoToMeadow)
            executingState = ExecutingAnimalState.Flee;
    }

    public void Flee()
    {
        distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < detectionRadius)
        {
            StartFlee();
        }
    }

    [HideInInspector] public Transform moveAroundCenter;
    public void StartMove()
    {
        if (_playerTransform != null)
        {
            OnWalk.Invoke();
            Agent.SetDestination(GetRandomPos(moveAroundCenter.position, 7f));  // 15f
        }

    }

    protected float distanceToPlayer;
    Vector3 fleeDirection;
    Vector3 fleePosition;

    public void StartFlee()
    {
        if (meadow != null && executingState == ExecutingAnimalState.GoToMeadow)
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
            OnGetScared?.Invoke();
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

    public void CheckDistanceToPlayer()
    {
        distanceToPlayer = Vector3.Distance(transform.position, _playerTransform.position);
        if (distanceToPlayer < detectionRadius)
        {
            executingState = ExecutingAnimalState.Flee;
        }
    }

    public void Graze()
    {
        _grazeTimer -= Time.deltaTime;
        if (_grazeTimer <= 0f)
        {
            executingState = ExecutingAnimalState.MoveAround;
        }
    }

    public abstract void FindNearestHerd();

    protected float cowDetectionRadius = 4f;
    protected Vector3 herdDirection;
    protected int neighborCount;
    public void FollowHerd()
    {
        Vector3 finalDirection = Vector3.zero;
        Vector3 fleeDirection = GetFleeDirection();
        herdDirection /= neighborCount;
        if (fleeDirection != Vector3.zero)
        {
            finalDirection = fleeDirection + herdDirection;
        }
        else if (herdDirection != Vector3.zero)
        {
            finalDirection = herdDirection;
        }
        Vector3 herdPosition = transform.position + finalDirection.normalized * moveRadius;
        Agent.SetDestination(herdPosition);

        Collider[] nearbyCows = Physics.OverlapSphere(transform.position, cowDetectionRadius);
        if (nearbyCows.Length! > 0)
        {
            executingState = ExecutingAnimalState.Graze;
        }
    }

    public void StartMoveToMeadow()
    {
        RejoinHerd();
    }

    public void RejoinHerd()
    {
        if (meadow != null)
        {
            executingState = ExecutingAnimalState.GoToMeadow;
            moveAroundCenter = meadow;
        }
    }

    public void SettleInBarn()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            if (hit.collider.CompareTag("Barn"))
            {
                Agent.SetDestination(GetRandomPos(hit.transform.position + Vector3.back * 2, 1.55f));
                executingState = ExecutingAnimalState.Rest;
            }
        }
    }

    public abstract void GetUsed(KeyCode interactKey);

    public Vector3 GetRandomPos(Vector3 center, float range)
    {
        randomPoint = center + UnityEngine.Random.insideUnitSphere * range;
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas);

        return hit.position;
    }

    public void StandIdle(float duration)
    {
        executingState = ExecutingAnimalState.DoNothing;

        Invoke("ChangeUIElement", duration);
    }

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

    [ContextMenu("Execute Take Damage Manually in editor")]
    void ManuelDamage()
    {
        TakeDamage(5);
    }
    public void TakeDamage(float amount)
    {
        if (!isAlive)
        {
            DecreaseItemCount();
            return;
        }
        hitPoint -= amount;
        CheckIsDead();

        //Particle
        ParticleManager.Instance.PlayParticle(ParticleType.BloodSpill, transform, transform.position + Vector3.up);
    }

    private void CheckIsDead()
    {
        if (hitPoint <= 0)
        {
            Die();
        }
        else
        {
            OnHurt?.Invoke();
            UpdateHitPointUI();
        }
    }

    private void UpdateHitPointUI()
    {
        hitPointUI.fillAmount = hitPoint / hitPointMaximum;
    }

    void Die()
    {
        //OnDie?.Invoke();
        isAlive = false;
        GetComponent<Collider>().enabled = true;
        GetComponent<Collider>().isTrigger = true;
        //Agent.enabled = false;

        hitPointUI.fillAmount = 0;

        executingState = ExecutingAnimalState.Dead;


        //Particle and Voice
        ParticleManager.Instance.PlayParticle(ParticleType.Die, null, transform.position);
    }

    public void SwitchState(AnimalStates nextState)
    {
        if (currentState != nextState)
        {
            currentState = nextState;
            currentState.EnterState(this);
        }
    }
}
