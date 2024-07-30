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
        agent.destination = dest;

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

            }
        }
    }

    void EnablePetting()
    {
        pettable = true;
        InteractableUIElements[0].EnableIt(0);
    }
}