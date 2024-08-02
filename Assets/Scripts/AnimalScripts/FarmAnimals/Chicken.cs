using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Chicken : MonoBehaviour
{
    [SerializeField] float minSpawRate = 10;

    [SerializeField] float maxSpawRate = 50;

    [SerializeField] GameObject eggPrefab;

    [SerializeField] private float wanderRadius = 10f;
    public float wanderTimer = 5f;

    public Transform target;
    private NavMeshAgent agent;
    private Animator animator;
    private float timer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        timer = wanderTimer;
        StartCoroutine(SpawnEgg());
    }



    IEnumerator SpawnEgg()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawRate, maxSpawRate));
            Instantiate(eggPrefab, transform.position, Quaternion.identity);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            animator.SetTrigger("Eat");
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chicken"))
        {
            animator.SetTrigger("Bounce");
            target = other.transform;
            Vector3 fleeDirection = (transform.position - target.position).normalized;
            Vector3 newFleePos = transform.position + fleeDirection * wanderRadius;
            agent.SetDestination(newFleePos);
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }
}
