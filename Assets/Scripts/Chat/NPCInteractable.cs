using UnityEngine;

public class NPCInteractable : Interactable
{
    [SerializeField] protected Animator animator;
    [SerializeField] protected NPCHeadLookAt npcHeadLookAt;
    [SerializeField] protected string textToSay = "Hello World";

    public void Talk(Transform interactor, IconType iconType, string textToSay)
    {
        ChatBubble.Create(this.transform, new Vector3(.7f, 2.1f), iconType, textToSay);
        animator.SetTrigger("Talk");
        npcHeadLookAt.LookAt(interactor);
    }
    public override void Interact(Transform interactorTransform, KeyCode keyCode)
    {
        if ((int)keyCode == (int)InteractKeys.Talk)
        {
            Talk(interactorTransform, IconType.Informative, textToSay);
        }


    }
}
