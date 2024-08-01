using System;
using System.Collections;
using UnityEngine;

public class CowController : AnimalBase
{

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        currentState.Interact(this, interactKey);
    }

    public  override void AvoidOtherAnimals()
    {
        Collider[] nearbyCows = Physics.OverlapSphere(transform.position, cowDetectionRadius);
        foreach (Collider cow in nearbyCows)
        {
            if (cow.gameObject != this.gameObject && cow.CompareTag("Cow"))
            {
                Vector3 avoidDirection = (transform.position - cow.transform.position).normalized;
                Vector3 avoidPosition = transform.position + avoidDirection * moveRadius;
                Agent.SetDestination(avoidPosition);
            }
        }
    }
    
    public override void FindNearestHerd()
    {
        _herdHeartbeat -= Time.deltaTime;

        herdDirection = Vector3.zero;
        neighborCount = 0;

        Collider[] nearbyCows = Physics.OverlapSphere(transform.position, cowDetectionRadius);
        foreach (Collider cow in nearbyCows)
        {
            if (cow.gameObject != this.gameObject && cow.CompareTag("Cow"))
            {
                herdDirection += cow.transform.forward;
                neighborCount++;
            }
        }

        if (neighborCount > 0)
        {
            executingState = ExecutingAnimalState.FollowHerd;
        }

        _herdHeartbeat = _maxDuration;
    }

    public void GetMilked(KeyCode interactKey)
    {
        if ((int)interactKey == (int)InteractKeys.InteractAnimals)
        {
            IPlayer = _playerTransform.GetComponentInParent<IPlayer>();
            if (IPlayer != null)
            {
                IPlayer.Milk(transform);
            }
        }
    }

    public override void GetUsed(KeyCode interactKey)
    {
        GetMilked(interactKey);
    }

    protected override void Die()
    {
        base.Die();

        SoundManager.Instance.PlaySound(VoiceType.CowNormal, transform, transform.position);
    }
}
