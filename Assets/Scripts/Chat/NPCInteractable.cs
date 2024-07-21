using UnityEngine;

public class NPCInteractable : Interactable
{
    [SerializeField] Animator animator;
    [SerializeField] NPCHeadLookAt npcHeadLookAt;

    public void Talk(Transform interactor)
    {
        ChatBubble.Create(this.transform, new Vector3(.7f, 2.1f), IconType.Informative, "Hoşgeldin Paşaa");
        animator.SetTrigger("Talk");
        npcHeadLookAt.LookAt(interactor);
    }
    public override void Interact(Transform interactorTransform)
    {
        Talk(interactorTransform);
    }
}
