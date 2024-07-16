using System;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBase : MonoBehaviour
{
    [HideInInspector] public Action OnWalk;
    [HideInInspector] public Action OnIdle;

    //public AnimalStates currentState;

    protected Transform _player;
    protected IPlayer IPlayer;

    [HideInInspector] public NavMeshAgent Agent;

    public virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    //void Update()
    //{
    //    currentState.UpdateState(this);
    //}

    //public void SwitchState(AnimalStates nextState)
    //{
    //    currentState = nextState;
    //    currentState.EnterState(this);
    //}
}
