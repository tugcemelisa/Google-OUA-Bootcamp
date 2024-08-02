using System;
using System.Collections.Generic;
using UnityEngine;
public class NPCQuestInteractable : NPCInteractable
{
    enum executingState
    {
        GiveQuest,
        Wait
    }
    executingState _executingState;

    [HideInInspector] public static Action<List<ItemData>> OnNpcBuy;

    [SerializeField] List<ItemData> neededItems = new List<ItemData>();

    float totalMoney = 0;

    public override void Start()
    {
        base.Start();
        _executingState = executingState.GiveQuest;

        float totalAmount = 0;
        for (int i = 0; i < neededItems.Count; i++)
        {
            totalAmount += neededItems[i].Type.value * neededItems[i].count;
        }
    }
    public override void Interact(Transform interactorTransform, KeyCode keyCode)
    {
        if (_executingState == executingState.Wait)
            return;
        else if (_executingState == executingState.GiveQuest)
        {
            if ((int)keyCode == (int)InteractKeys.Bargain)
                Bargain(interactorTransform);

            else if ((int)keyCode == (int)InteractKeys.Talk)
            {
                ShowQuest(interactorTransform, IconType.Bargain, textToSay + " I can give you " + totalMoney + "$ for those:");
            }
        }
    }

    private void ShowQuest(Transform interactor, IconType iconType, string textToSay)
    {
        BargainBubble.Create(this.transform, new Vector3(.7f, 2.1f), iconType, textToSay, neededItems);
        animator.SetTrigger("Talk");
        npcHeadLookAt.LookAt(interactor);
    }

    private void RemoveItemsInInventory()
    {
        InventoryManager.Instance.RemoveItems(neededItems);
    }

    private bool playerHasNeededItems()
    {
        return InventoryManager.Instance.HasItems(neededItems);
    }

    public void Bargain(Transform interactorTransform)
    {
        if (playerHasNeededItems())
        {
            OnNpcBuy.Invoke(neededItems);
            RemoveItemsInInventory();
            Talk(interactorTransform, IconType.Success, " The quest has been completed succesfully. \n" +
                "Thank for seeling me those items and here is your money.");

            foreach (var item in InteractableUIElements)
            {
                if (item.enabled && item.InteractKey == InteractKeys.Talk)
                {
                    item.enabled = false;
                    PlayerInteractableUI.Instance.UpdateUIElements();
                }
                if (item.enabled && item.InteractKey == InteractKeys.Bargain)
                {
                    item.Disable(true);

                    PlayerInteractableUI.Instance.UpdateUIElements();
                    break;
                }
            }

            print("YOU GAINED MONEY");
            _executingState = executingState.Wait;

            //Sound

            SoundManager.Instance.PlaySound(VoiceType.Success, null, transform.position);
        }
        else
        {
            Talk(interactorTransform, IconType.Failure, " The items in the list are not exist in your inventory. \n" +
                "Come back when you have them all");
        }
    }
}