using UnityEngine;
using UnityEngine.AI;

public class DogController : Interactable
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] Animator aiAnim;
    Vector3 dest;

    [SerializeField] float pettingDelay = 15f;
    bool pettable = true;

    [SerializeField] Transform playerPos;

    void Update()
    {
        dest = player.position;
        if (Vector3.Distance(dest, transform.position) > agent.stoppingDistance)
        {
            agent.destination = dest;
        }else
        {
            agent.destination = transform.position;
        }

        float speed = agent.velocity.magnitude;
        if (speed > 0)
        {
            aiAnim.SetFloat("Speed", speed / agent.speed * 6);
        }

    }

    TorchController Player;
    public override void Interact(Transform interactorTransform, KeyCode interactKey)
    {
        if (pettable && (int)interactKey == (int)InteractKeys.InteractAnimals)
        {
            Player = player.GetComponentInParent<TorchController>();
            if (Player != null)
            {
                Player.transform.position = playerPos.position;
                Player.Pet(transform);

                InteractableUIElements[0].Disable(false);           
                PlayerInteractableUI.Instance.UpdateUIElements();   

                pettable = false;
                Invoke("EnablePetting", pettingDelay);


                agent.isStopped = true;
                Invoke("Resume", 5);

            }
        }
    }

    void EnablePetting()
    {
        pettable = true;
        InteractableUIElements[0].EnableIt(0);
    }

    void Resume()
    {
        agent.isStopped = false;
    }
}