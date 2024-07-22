using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviourSingletonPersistent<InventoryManager>
{
    [SerializeField] private List<InventorySlot> slots;

    [SerializeField] private InventorySlot selectedSlot;

    [SerializeField]
    private Transform handTransform;

    [SerializeField] private Animator animator;

    private void Start()
    {
        ShowInventory();
    }

    private void Update()
    {
        KeyCode keyCode = SelectSlot();
        if (keyCode == KeyCode.None)
        {
            return;
        }
        ChooseSlot(keyCode);
    }

    void ChooseSlot(KeyCode input)
    {
        if (selectedSlot != null)
        {
            selectedSlot.Deselect();
        }

        foreach (var slot in slots)
        {
            if (slot.KeyCode == input)
            {
                selectedSlot = slot;
                selectedSlot.Select();
            }
        }
    }

    public void AddItem(ItemMonoBehaviour itemMonobehaviour)
    {

        //Already has that type of item, then increase the count
        foreach (InventorySlot slot in slots)
        {

            if (slot.isSlotEmpty()) continue;

            ItemData slotItemData = slot.GetItem().ItemData;
            if (slotItemData.Type == itemMonobehaviour.ItemData.Type)
            {
                slotItemData.count += itemMonobehaviour.ItemData.count;

                slot.UpdateItem();

                Destroy(itemMonobehaviour.gameObject);
                return;
            }
        }

        //If dont have that type of item, then add it
        bool hasEmptySlot = false;
        foreach (InventorySlot slot in slots)
        {
            if (slot.isSlotEmpty())
            {
                hasEmptySlot = true;
                slot.SetItem(itemMonobehaviour);

                itemMonobehaviour.GetComponent<Collider>().enabled = false;
                itemMonobehaviour.gameObject.SetActive(false);
                itemMonobehaviour.transform.SetParent(this.handTransform);
                itemMonobehaviour.gameObject.transform.localPosition = Vector3.zero;
                break;
            }
        }

        if (!hasEmptySlot)
        {
            print("DO NOT HAVE EMPTHY SLOT");
        }
    }

    private void RemoveItem(ItemType itemType, int count)
    {
        int countToBeRemoved = count;
        foreach (InventorySlot slot in slots)
        {
            if (countToBeRemoved <= 0)
            {
                break;
            }

            if (slot.isSlotEmpty())
                continue;

            ItemData data = slot.GetItem().ItemData;
            if (data.Type == itemType)
            {
                if (data.count >= countToBeRemoved)
                {
                    data.count -= countToBeRemoved;
                }
                else
                {
                    countToBeRemoved -= data.count;
                    data.count = 0;
                }

                slot.UpdateItem();
            }
        }
    }

    private bool HasItem(ItemType itemType, int count)
    {
        int existCount = 0;
        foreach (InventorySlot slot in slots)
        {
            if (existCount >= count)
            {
                return true;
            }

            if (slot.isSlotEmpty())
                continue;

            ItemData slotData = slot.GetItem().ItemData;
            if (slotData.Type == itemType)
            {
                existCount += slotData.count;
            }
        }

        return existCount >= count;
    }

    public bool HasItems(List<ItemData> items)
    {
        foreach (ItemData item in items)
        {
            if (!HasItem(item.Type, item.count))
            {
                return false;
            }
        }
        return true;

    }

    public void RemoveItems(List<ItemData> items)
    {
        foreach (ItemData item in items)
        {
            RemoveItem(item.Type, item.count);
        }
    }

    public void ShowInventory()
    {
        foreach (var slot in slots)
        {
            slot.gameObject.SetActive(!slot.isSlotEmpty());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            ItemMonoBehaviour item = other.GetComponent<ItemMonoBehaviour>();


            AddItem(item);
            ShowInventory();
        }
    }

    public void HoldItemAnimation(ItemMonoBehaviour item)
    {
        item.gameObject.SetActive(true);
        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;

        animator.SetTrigger("Hold");
    }
    public void RemoveItemAnimation()
    {
        animator.SetTrigger("FinishHolding");
    }

    public KeyCode SelectSlot()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { return KeyCode.Alpha1; }
        if (Input.GetKeyDown(KeyCode.Alpha2)) { return KeyCode.Alpha2; }
        if (Input.GetKeyDown(KeyCode.Alpha3)) { return KeyCode.Alpha3; }
        if (Input.GetKeyDown(KeyCode.Alpha4)) { return KeyCode.Alpha4; }
        if (Input.GetKeyDown(KeyCode.Alpha5)) { return KeyCode.Alpha5; }
        if (Input.GetKeyDown(KeyCode.Alpha6)) { return KeyCode.Alpha6; }
        if (Input.GetKeyDown(KeyCode.Alpha7)) { return KeyCode.Alpha7; }
        return KeyCode.None;
    }


}
