using UnityEngine;

public class DogHeadIK : MonoBehaviour
{
    [SerializeField] Animator animator;

    [SerializeField] Transform lookAtObj;

    [SerializeField] bool isIKActive =false;


    private void OnAnimatorIK()
    {
        if(animator)
            if(isIKActive && lookAtObj)
            {
                animator.SetLookAtWeight(1f);
                animator.SetLookAtPosition(lookAtObj.position);
            }
            else
            {

                animator.SetLookAtWeight(0f);
            }

    }
}
