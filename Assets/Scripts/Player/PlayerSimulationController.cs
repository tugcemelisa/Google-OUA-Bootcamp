using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

enum PlayerStates
{
    Default,
    TakeAnimals,
    Milking,
    HoldingMilkPail,
    Selling,
    Shear,
    HoldingWool,
    Fight
}
public class PlayerSimulationController : MonoBehaviour, IPlayer
{
    private Animator _animator;
    public RuntimeAnimatorController daytimeAnimator;
    public RuntimeAnimatorController nightAnimator;
    PlayerStates _executingState;
    IFarmAnimal _milkable;
    TorchController _torchController;
    [SerializeField] private Transform pailAnchorTransform;
    [SerializeField] private Transform milkPailTransform;
    [SerializeField] private GameObject milkPailPrefab;
    [SerializeField] private ParticleSystem cutWoolEffect;

    private List<AnimalBase> _herd = new();
    [HideInInspector] public static Action<List<AnimalBase>> OnTranshumingStart;
    [HideInInspector] public static Action OnHerdLeaveBarn;

    private void OnEnable()
    {
        GameModeManager.OnNightStart += ActivatePlayerNightMode;
    }
    private void OnDisable()
    {
        GameModeManager.OnNightStart -= ActivatePlayerNightMode;
    }

    private void Start()
    {
        _torchController = GetComponent<TorchController>();
        _animator = GetComponent<Animator>();
        _animator.runtimeAnimatorController = daytimeAnimator;
        _executingState = PlayerStates.Default;
    }

    private void Update()
    {
        switch (_executingState)
        {
            case PlayerStates.TakeAnimals:
                StartGrazing();
                break;
            case PlayerStates.HoldingWool:
                HoldWool();
                break;
            case PlayerStates.HoldingMilkPail:
                SellRequest(); 
                break;
            case PlayerStates.Fight:
                _torchController.ControlAttack();
                break;
            default:
                break;
        }
    }

    public void TakeAnimals(List<AnimalBase> animals)
    {
        _herd.AddRange(animals);

        int priority = 50;
        for (int i = 0; i < _herd.Count; i++)
        {
            _herd[i].Agent.avoidancePriority = priority;
            priority++;
        }
        OnTranshumingStart.Invoke(_herd);
        _executingState = PlayerStates.TakeAnimals;
    }

    public void StartGrazing()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 5f))
        {
            if (hit.collider.CompareTag("Barn"))
            {
                _executingState = PlayerStates.Default;
                OnHerdLeaveBarn.Invoke();
            }
        }
    }

    public void ScareAnimal(NavMeshAgent animalAgent)
    {
        InputTrigger("Scare");
    }

    Transform _milkingAnimal;
    public void Milk(Transform milkingAnimal)
    {
        _milkingAnimal = milkingAnimal;
        _milkable = _milkingAnimal.GetComponent<IFarmAnimal>();
        InputTrigger("Crouch");
        _holdingPail = Instantiate(milkPailPrefab, _milkingAnimal.position + new Vector3(0.3f, 0, 0), Quaternion.identity);

        if (_milkable != null)
            _milkable.StandIdle(3.5f);

        StartCoroutine(FinishMilking());
    }

    private IEnumerator FinishMilking()
    {
        yield return new WaitForSeconds(3.5f);

        InputTrigger("FinishCrouching");
        HoldMilkPail();
        _executingState = PlayerStates.HoldingMilkPail;
    }

    GameObject _holdingPail;
    public void HoldMilkPail()
    {
        InputTrigger("Hold");
        _holdingPail.transform.position = new Vector3(0, -0.5f, 0);
        _holdingPail.transform.SetParent(milkPailTransform, false);
        _holdingPail.transform.GetChild(1).GetComponent<HingeJoint>().connectedBody = pailAnchorTransform.GetComponent<Rigidbody>();
    }

    private void SellRequest()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            InputTrigger("FinishHolding");
            _holdingPail.transform.SetParent(null);
            //_holdingPail.GetComponent<Rigidbody>().useGravity = true;
            //_holdingPail.GetComponent<Rigidbody>().isKinematic = false;
            _executingState = PlayerStates.Default;
        }
    }

    public void StartMilk()
    {
        if(_executingState == PlayerStates.Default)
        {
            _executingState = PlayerStates.Milking; 
        }
            
    }

    public void Shear(Transform sheepTransform)
    {
        if (_executingState == PlayerStates.Default)
        {
            _executingState = PlayerStates.Shear;
        }

        _milkingAnimal = sheepTransform;
        _milkable = _milkingAnimal.GetComponent<IFarmAnimal>();
        InputTrigger("Crouch");
        InputTrigger("HoldingDown");

        if (_milkable != null)
            _milkable.StandIdle(5.5f);

        StartCoroutine(FinishShearing());
    }

    private IEnumerator FinishShearing()
    {
        yield return new WaitForSeconds(5.5f);

        InputTrigger("FinishCrouching");
        cutWoolEffect.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        _executingState = PlayerStates.HoldingWool;
    }
    private void HoldWool()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            InputTrigger("FinishHolding");
            _executingState = PlayerStates.Default;
        }
    }

    private void ActivatePlayerNightMode()
    {
        _animator.runtimeAnimatorController = nightAnimator;
        _executingState = PlayerStates.Fight;
    }

    private void InputTrigger(string trigger)
    {
        _animator.SetTrigger(trigger);
    }

    #region Animation Event Methods
    public void StartMilkAnimation()
    {
        InputTrigger("Milk");
    }

    public void StartCuttingAnimation()
    {
        InputTrigger("Cut");
        cutWoolEffect.Play();
    }

    public void FinishScare()
    {
        InputTrigger("FinishScare");
    }
    #endregion
}
