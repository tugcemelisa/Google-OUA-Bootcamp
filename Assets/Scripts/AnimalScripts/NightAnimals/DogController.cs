using UnityEngine;
using UnityEngine.AI;

public class DogController : MonoBehaviour
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform player;
    [SerializeField] Animator aiAnim;
    Vector3 dest;

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
}