using System;
using UnityEngine;
using UnityEngine.AI;

public class AnimalBase : MonoBehaviour
{
    [HideInInspector] public Action OnWalk;
    [HideInInspector] public Action OnIdle;

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
}
