using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Cow : MonoBehaviour
{
    //public Transform shepherd;
    //public float detectionRadius = 10f;
    //public float moveRadius = 5f;
    //public float avoidRadius = 2f;
    //public float cowDetectionRadius = 3f;
    //public float herdRadius = 20f;
    //public float grazingRadius = 15f;
    //public float grazingInterval = 5f;
    //private NavMeshAgent agent;

    //private Vector3 herdCenter;
    //private float grazingTimer;
    //private List<Transform> otherCows = new List<Transform>();

    //private void Start()
    //{
    //    agent = GetComponent<NavMeshAgent>();
    //    herdCenter = transform.position;
    //    grazingTimer = grazingInterval;
    //}

    //private void Update()
    //{
    //    FleeFromShepherd();
    //    AvoidOtherCows();
    //    Graze();
    //    FollowHerd();
    //}

    //private void FleeFromShepherd()
    //{
    //    float distanceToShepherd = Vector3.Distance(transform.position, shepherd.position);
    //    if (distanceToShepherd < detectionRadius)
    //    {
    //        Vector3 fleeDirection = (transform.position - shepherd.position).normalized;
    //        Vector3 fleePosition = transform.position + fleeDirection * moveRadius;
    //        agent.SetDestination(fleePosition);
    //    }
    //}

    //private void AvoidOtherCows()
    //{
    //    Collider[] nearbyCows = Physics.OverlapSphere(transform.position, cowDetectionRadius);
    //    foreach (Collider cow in nearbyCows)
    //    {
    //        if (cow.gameObject != this.gameObject && cow.CompareTag("Cow"))
    //        {
    //            Vector3 avoidDirection = (transform.position - cow.transform.position).normalized;
    //            Vector3 avoidPosition = transform.position + avoidDirection * avoidRadius;
    //            agent.SetDestination(avoidPosition);
    //        }
    //    }
    //}

    //private void Graze()
    //{
    //    grazingTimer -= Time.deltaTime;
    //    if (grazingTimer <= 0)
    //    {
    //        Vector3 grazingPosition = GetRandomGrazingPosition();
    //        agent.SetDestination(grazingPosition);
    //        grazingTimer = grazingInterval;
    //    }
    //}

    //private Vector3 GetRandomGrazingPosition()
    //{
    //    Vector3 randomDirection = Random.insideUnitSphere * grazingRadius;
    //    randomDirection += herdCenter;
    //    NavMeshHit hit;
    //    NavMesh.SamplePosition(randomDirection, out hit, grazingRadius, 1);
    //    Vector3 finalPosition = hit.position;
    //    return finalPosition;
    //}

    //private void FollowHerd()
    //{
    //    Vector3 herdDirection = Vector3.zero;
    //    int neighborCount = 0;

    //    Collider[] nearbyCows = Physics.OverlapSphere(transform.position, cowDetectionRadius);
    //    foreach (Collider cow in nearbyCows)
    //    {
    //        if (cow.gameObject != this.gameObject && cow.CompareTag("Cow"))
    //        {
    //            herdDirection += cow.transform.forward;
    //            neighborCount++;
    //        }
    //    }

    //    if (neighborCount > 0)
    //    {
    //        herdDirection /= neighborCount;
    //        Vector3 herdPosition = transform.position + herdDirection.normalized * moveRadius;
    //        agent.SetDestination(herdPosition);
    //    }
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(herdCenter, herdRadius);
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(herdCenter, grazingRadius);
    //}

    private Transform shepherd;
    public float detectionRadius = 10f;
    public float moveRadius = 5f;
    public float avoidRadius = 2f;
    public float cowDetectionRadius = 3f;
    public float herdRadius = 20f;
    public float grazingRadius = 15f;
    public float grazingInterval = 5f;
    private NavMeshAgent agent;

    private Vector3 herdCenter;
    private float grazingTimer;

    private void Start()
    {
        herdCenter = transform.position;
        grazingTimer = grazingInterval;
        agent = GetComponent<NavMeshAgent>();
        shepherd = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    private void Update()
    {
        Vector3 finalDirection = Vector3.zero;
        int neighborCount = 0;

        // Çobandan kaçma yönü
        Vector3 fleeDirection = GetFleeDirection();

        // Sürü yönü
        Vector3 herdDirection = Vector3.zero;
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
            herdDirection /= neighborCount;
        }

        // Nihai yönü belirle
        if (fleeDirection != Vector3.zero)
        {
            finalDirection = fleeDirection + herdDirection;
        }
        else if (herdDirection != Vector3.zero)
        {
            finalDirection = herdDirection;
        }
        else
        {
            Graze();
            return;
        }

        Vector3 destination = transform.position + finalDirection.normalized * moveRadius;
        agent.SetDestination(destination);
    }

    private Vector3 GetFleeDirection()
    {
        float distanceToShepherd = Vector3.Distance(transform.position, shepherd.position);
        if (distanceToShepherd < detectionRadius)
        {
            return (transform.position - shepherd.position).normalized;
        }
        return Vector3.zero;
    }

    private void Graze()
    {
        grazingTimer -= Time.deltaTime;
        if (grazingTimer <= 0)
        {
            Vector3 grazingPosition = GetRandomGrazingPosition();
            agent.SetDestination(grazingPosition);
            grazingTimer = grazingInterval;
        }
    }

    private Vector3 GetRandomGrazingPosition()
    {
        Vector3 randomDirection = Random.insideUnitSphere * grazingRadius;
        randomDirection += herdCenter;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, grazingRadius, 1);
        Vector3 finalPosition = hit.position;
        return finalPosition;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(herdCenter, herdRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(herdCenter, grazingRadius);
    }
}

