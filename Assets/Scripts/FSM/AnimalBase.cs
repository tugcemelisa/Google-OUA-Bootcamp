using System;
using UnityEngine;
using UnityEngine.AI;

public abstract class AnimalBase : Interactable, IFarmAnimal
{
    #region Actions
    [HideInInspector] public Action OnWalk;
    [HideInInspector] public Action OnIdle;
    [HideInInspector] public Action OnGraze;
    [HideInInspector] public Action OnGrazeFinish;
    [HideInInspector] public Action OnFlee;
    #endregion

    //public AnimalStates currentState;

    protected Transform _playerTransform;
    protected IPlayer IPlayer;

    [HideInInspector] public NavMeshAgent Agent;

    [HideInInspector] public float acceleration;
    [HideInInspector] public float speed;

    public virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        _playerTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();

        acceleration = Agent.acceleration;
        speed = Agent.speed;
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
}
