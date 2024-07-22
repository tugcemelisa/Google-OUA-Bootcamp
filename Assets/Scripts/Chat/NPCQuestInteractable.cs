using System;
using System.Collections.Generic;
using UnityEngine;
public class NPCQuestInteractable : NPCInteractable
{
    [SerializeField] List<ItemData> neededItems = new List<ItemData>();
    public override void Interact(Transform interactorTransform, KeyCode keyCode)
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
            Talk(interactorTransform, IconType.Informative, textToSay + itemList);
        }
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
            Talk(interactorTransform, IconType.Success, " The quest has been completed succesfully. Thank for seeling me those items and here is your money.");

            print("YOU GAINED MONEY");
        }
        else
        {
            Talk(interactorTransform, IconType.Failure, " The items in the list are not exist in your inventory. Come back when you have them all");
        }
    }
}