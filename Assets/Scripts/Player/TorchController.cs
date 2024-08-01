using System;
using UnityEngine;

public class TorchController : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] float attackCooldown = 1f;
    float cooldown = 0;
    bool canAttack = true;

    [SerializeField] float fearPerAttack = 5f;

    [SerializeField] Transform torchHead;
    [SerializeField] float fearAttackRadius = 3f;
    [SerializeField] LayerMask layerToGiveFear;


    private void OnEnable()
    {
        GameModeManager.OnNightStart += () => torchHead.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        GameModeManager.OnNightStart -= () => torchHead.gameObject.SetActive(true);
    }

    private void Start()
    {
        torchHead.gameObject.SetActive(false);
    }

    public void ControlAttack()
    {
        cooldown -= Time.deltaTime;
        if (cooldown <= 0)
        {
            cooldown = 0;
            canAttack = true;
        }

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            canAttack = false;
            cooldown = attackCooldown;
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            StarterAssets.InputController.Instance.DisableInputs();
            animator.SetTrigger("Gather");
        }
    }

    public void Pet(Transform dog)
    {
        StarterAssets.InputController.Instance.DisableInputs();
        animator.SetTrigger("Petting");
        transform.LookAt(dog);
    }

    private void Attack()
    {
        animator.SetTrigger("Attack");
        GiveFear();

        //Sound
        VoiceType type = UnityEngine.Random.Range(0, 10) > 5f ? VoiceType.TorchAttack : VoiceType.TorchAttack2;
        SoundManager.Instance.PlaySound(type, transform, transform.position);
    }

    private void GiveFear()
    {
        var rayCastHits = Physics.SphereCastAll(torchHead.position, fearAttackRadius, -Vector3.up, layerToGiveFear);

        foreach (var hit in rayCastHits)
        {
            var wolf = hit.collider.GetComponent<WolfController>();
            if (wolf)
            {
                wolf.AddFear(fearPerAttack);
                Debug.Log(name + " scares " + wolf.name + " TORCHCONTROLLER");


                //Sound
                SoundManager.Instance.PlaySound(VoiceType.WolfHurt, wolf.transform, wolf.transform.position);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;

        Gizmos.DrawWireSphere(torchHead.position, fearAttackRadius);
    }
}
