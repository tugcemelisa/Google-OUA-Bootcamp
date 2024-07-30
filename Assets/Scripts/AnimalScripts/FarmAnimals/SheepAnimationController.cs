using UnityEngine;

public class SheepAnimationController : MonoBehaviour
{
    Animator animator;
    Animator Animator
    {
        get
        {
            if (animator == null)
                return animator = GetComponent<Animator>();
            else
                return animator;
        }
    }

    SheepController sheepController;
    SheepController SheepController
    {
        get
        {
            if (sheepController == null)
                return sheepController = GetComponentInParent<SheepController>();
            else
                return sheepController;
        }
    }

    private void OnEnable()
    {
        SheepController.OnWalk += () => InputTrigger("Walk");
        SheepController.OnGetScared += () => InputTrigger("GetScared");
        SheepController.OnGraze += () => InputTrigger("Graze");
        SheepController.OnRun += () => InputTrigger("Run");
        SheepController.OnIdle += () => InputTrigger("Idle");
        SheepController.OnSit += () => InputTrigger("Sit");
        SheepController.OnDie += () => InputTrigger("Dead");
    }
    private void OnDisable()
    {
        SheepController.OnWalk -= () => InputTrigger("Walk");
        SheepController.OnGetScared -= () => InputTrigger("GetScared");
        SheepController.OnGraze -= () => InputTrigger("Graze");
        SheepController.OnRun -= () => InputTrigger("Run");
        SheepController.OnIdle -= () => InputTrigger("Idle");
        SheepController.OnSit -= () => InputTrigger("Sit");
        SheepController.OnDie -= () => InputTrigger("Dead");
    }

    private void InputTrigger(string trigger)
    {
        Animator.SetTrigger(trigger);
    }
}
