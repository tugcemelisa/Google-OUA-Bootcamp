using DG.Tweening;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public enum NpcState
{
    WalkAround,
    Wait
}
public class NpcController : MonoBehaviour, INpc
{
    public NpcState executingState;
    private NavMeshAgent Agent;
    private NavMeshHit hit;
    Animator animator;

    public Transform _marketplace;
    private Transform _player;
    private Vector3 randomPoint;

    public virtual void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        _player = GameObject.FindWithTag("Player").GetComponent<Transform>(); // Player tag'i olan nesneyi bulur
        executingState = NpcState.WalkAround; // Ba�lang��ta NPC y�r�r
    }

    public void Update()
    {
        switch (executingState)
        {
            case NpcState.WalkAround:
                MoveAround();
                break;
            case NpcState.Wait:
                Wait();
                break;
        }
    }

    private void MoveAround()
    {
        if (!Agent.hasPath)
        {
            if (_marketplace != null)
            {
                Agent.SetDestination(GetRandomPos(_marketplace.position, 15f)); // NPC belirli bir alan i�inde hareket eder
                animator.SetTrigger("Walk"); // Animasyonu tetikler
            }
        }
    }

    public void Talk()
    {
        if (executingState != NpcState.Wait)
        {
            RotateToPlayer();
            executingState = NpcState.Wait; // NPC bekleme moduna ge�er
            animator.SetTrigger("Idle"); // Bekleme animasyonu tetiklenir
        }
    }

    private void Wait()
    {
        Agent.SetDestination(transform.position); // Hareket durdurulur, NPC bekler

        // Burada playerInteract'i kald�rd�k ��nk� mevcut kodda tan�ml� de�il.
        // Bu k�sm� kendi etkile�im sisteminize g�re tekrar yap�land�rabilirsiniz.
        // executingState = NpcState.WalkAround; NPC bekleme durumundan y�r�y��e ge�er.
    }

    public Vector3 GetRandomPos(Vector3 center, float range)
    {
        randomPoint = center + Random.insideUnitSphere * range; // Rastgele bir pozisyon belirler
        NavMesh.SamplePosition(randomPoint, out hit, range, NavMesh.AllAreas); // NavMesh i�inde ge�erli pozisyonu bulur

        return hit.position;
    }

    public void RotateToPlayer()
    {
        if (_player != null)
        {
            Vector3 direction = (_player.position - transform.position).normalized; // Oyuncuya d�nme y�n�
            Vector3 targetEulerAngles = Quaternion.LookRotation(direction).eulerAngles; // D�nd�rme a��s�
            transform.DORotate(targetEulerAngles, 2); // D�nd�rme i�lemi DOTween ile 2 saniyede ger�ekle�tirilir
        }
    }
}