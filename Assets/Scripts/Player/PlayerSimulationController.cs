using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

enum PlayerStates
{
    Default,
    Milking,
    HoldingMilkPail,
    Selling,
    Shear,
    HoldingWool
}
public class PlayerSimulationController : MonoBehaviour, IPlayer
{
    Animator _animator;
    PlayerStates _executingState;
    IFarmAnimal _milkable;
    [SerializeField] private Transform milkPailTransform;
    [SerializeField] private GameObject milkPailPrefab;
    [SerializeField] private ParticleSystem cutWoolEffect;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _executingState = PlayerStates.Default;
    }

    private void Update()
    {
        switch (_executingState)
        {
            case PlayerStates.Shear:
                Shear();
                break;
            case PlayerStates.Milking:
                Milk();
                break;
            case PlayerStates.HoldingWool:
                HoldWool();
                break;
            case PlayerStates.HoldingMilkPail:
                SellRequest(); 
                break;
            default:
                break;
        }
    }

    public void ScareAnimal(NavMeshAgent animalAgent)
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            InputTrigger("Scare");
            animalAgent.acceleration = 16;
            animalAgent.speed = 3;
        }
    }

    Transform _milkingAnimal;
    public void Milk(Transform milkingAnimal)
    {
        _milkingAnimal = milkingAnimal;
    }
    private void Milk()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            float distanceToAnimal = Vector3.Distance(transform.position, _milkingAnimal.position);
            if (distanceToAnimal < 1.5f)
            {
                _milkable = _milkingAnimal.GetComponent<IFarmAnimal>();
                InputTrigger("Crouch");
                _holdingPail = Instantiate(milkPailPrefab, _milkingAnimal.position + new Vector3(0.3f, 0, 0), Quaternion.identity);

                if (_milkable != null)
                    _milkable.StandIdle();

                StartCoroutine(FinishMilking());
                return;
            }
            _executingState = PlayerStates.Default;
        }
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
        _holdingPail.transform.position = new Vector3(0, -0.12f, 0);
        _holdingPail.transform.SetParent(milkPailTransform, false);
    }

    private void SellRequest()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            InputTrigger("FinishHolding");
            _holdingPail.transform.SetParent(null);
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
        _milkingAnimal = sheepTransform;

        if (_executingState == PlayerStates.Default)
        {
            _executingState = PlayerStates.Shear;
        }
    }
    private void Shear()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            float distanceToAnimal = Vector3.Distance(transform.position, _milkingAnimal.position);
            if (distanceToAnimal < 1.5f)
            {
                _milkable = _milkingAnimal.GetComponent<IFarmAnimal>();
                InputTrigger("Crouch");
                InputTrigger("HoldingDown");

                if (_milkable != null)
                    _milkable.StandIdle();

                StartCoroutine(FinishShearing());
                return;
            }
            _executingState = PlayerStates.Default;
        }
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
