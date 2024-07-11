using UnityEngine;

public class CowAnimationController : MonoBehaviour
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

    CowController cowController;
    CowController CowController { 
        get 
        {
            if (cowController == null)
                return cowController = GetComponentInParent<CowController>();
            else
                return cowController; 
        } 
    }

    private void OnEnable()
    {
        CowController.OnWalk += () => InputTrigger("Walk");
        CowController.OnGraze += () => InputTrigger("Graze");
        CowController.OnFlee += () => InputTrigger("Flee");
        CowController.OnIdle += () => InputTrigger("Idle");
    }
    private void OnDisable()
    {
        CowController.OnWalk -= () => InputTrigger("Walk");
        CowController.OnGraze -= () => InputTrigger("Graze");
        CowController.OnFlee -= () => InputTrigger("Flee");
        CowController.OnIdle -= () => InputTrigger("Idle");
    }

    private void InputTrigger(string trigger)
    {
        Animator.SetTrigger(trigger);
    }

    #region Animation Event
    public void EndGraze()
    {
        //CowController.OnGrazeFinish.Invoke();
        Debug.Log("graze ended");
    }
    #endregion
}
