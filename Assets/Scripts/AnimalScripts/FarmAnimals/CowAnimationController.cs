using UnityEngine;

public class CowAnimationController : MonoBehaviour
{
    public RuntimeAnimatorController daytimeAnimator;
    public RuntimeAnimatorController nightAnimator;

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

    AnimalBase cowController;
    AnimalBase CowController { 
        get 
        {
            if (cowController == null)
                return cowController = GetComponentInParent<AnimalBase>();
            else
                return cowController; 
        } 
    }

    private void OnEnable()
    {
        GameModeManager.OnNightStart += () => Animator.runtimeAnimatorController = nightAnimator;
        WolfManager.OnHuntOver += () => Animator.runtimeAnimatorController = daytimeAnimator;

        CowController.OnWalk += () => InputTrigger("Walk");
        CowController.OnGetScared += () => InputTrigger("GetScared");
        CowController.OnGraze += () => InputTrigger("Graze");
        CowController.OnFlee += () => InputTrigger("Flee");
        CowController.OnIdle += () => InputTrigger("Idle");
        CowController.OnHurt += () => InputTrigger("Hurt");
        CowController.OnDie += () => InputTrigger("Dead");
    }
    private void OnDisable()
    {
        GameModeManager.OnNightStart -= () => Animator.runtimeAnimatorController = nightAnimator;
        WolfManager.OnHuntOver -= () => Animator.runtimeAnimatorController = daytimeAnimator;

        CowController.OnWalk -= () => InputTrigger("Walk");
        CowController.OnGetScared -= () => InputTrigger("GetScared");
        CowController.OnGraze -= () => InputTrigger("Graze");
        CowController.OnFlee -= () => InputTrigger("Flee");
        CowController.OnIdle -= () => InputTrigger("Idle");
        CowController.OnHurt -= () => InputTrigger("Hurt");
        CowController.OnDie -= () => InputTrigger("Dead");
    }

    private void Start()
    {
        Animator.runtimeAnimatorController = daytimeAnimator;
    }

    private void InputTrigger(string trigger)
    {
        Animator.SetTrigger(trigger);
    }
}
