﻿using System;
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

    [SerializeField] List<ItemData> neededItems = new List<ItemData>();

    public override void Start()
    {
        base.Start();
        _executingState = executingState.GiveQuest;
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
                string itemList = "";
                foreach (ItemData item in neededItems)
                {
                    itemList += item.Type.itemName + " : " + item.count.ToString();
                }
                ShowQuest(interactorTransform, IconType.Bargain, textToSay + " | " + itemList);
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
        }
        else
        {
            Talk(interactorTransform, IconType.Failure, " The items in the list are not exist in your inventory. \n" +
                "Come back when you have them all");
        }
    }
}