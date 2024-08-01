using System;
using System.Collections;
using UnityEngine;

public class SheepController : AnimalBase
{
    [HideInInspector] public Action OnRun;
    [HideInInspector] public Action OnSit;
    [HideInInspector] public Action OnRightTurn;
    [HideInInspector] public Action OnLeftTurn;

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        currentState.Interact(this, interactKey);
        Scream();
    }

    public float sheepDetectionRadius = 2f;
    public  override void FindNearestHerd()
    {
        _herdHeartbeat -= Time.deltaTime;

        if (_herdHeartbeat <= 0f)
        {
            herdDirection = Vector3.zero;
            neighborCount = 0;

            Collider[] nearbySheeps = Physics.OverlapSphere(transform.position, sheepDetectionRadius);
            foreach (Collider sheep in nearbySheeps)
            {
                if (sheep.gameObject != this.gameObject && sheep.CompareTag("Sheep"))
                {
                    herdDirection += sheep.transform.forward;
                    neighborCount++;
                }
            }

            if (neighborCount > 0)
            {
                executingState = ExecutingAnimalState.FollowHerd;
            }

            _herdHeartbeat = _maxDuration;
        }
    }

    public override void AvoidOtherAnimals()
    {
        Collider[] nearbyCows = Physics.OverlapSphere(transform.position, sheepDetectionRadius);
        foreach (Collider cow in nearbyCows)
        {
            if (cow.gameObject != this.gameObject && cow.CompareTag("Sheep"))
            {
                Vector3 avoidDirection = (transform.position - cow.transform.position).normalized;
                Vector3 avoidPosition = transform.position + avoidDirection * moveRadius;
                Agent.SetDestination(avoidPosition);
            }
        }
    }

    public void GetSheared(KeyCode interactKey)
    {
        if ((int)interactKey == (int)InteractKeys.InteractAnimals)
        {
            IPlayer = _playerTransform.GetComponentInParent<IPlayer>();
            if (IPlayer != null)
            {
                IPlayer.Shear(transform);
            }
        }
    }

    public override void GetUsed(KeyCode interactKey)
    {
        GetSheared(interactKey);
    }

    protected override void Die()
    {
        base.Die();
        Scream();
    }

    void Scream()
    {
        SoundManager.Instance.PlaySound(VoiceType.Sheep, transform, transform.position);
    }
}
