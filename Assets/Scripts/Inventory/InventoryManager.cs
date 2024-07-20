using System;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviourSingletonPersistent<InventoryManager>
{
    [SerializeField] private List<InventorySlot> slots;

    [SerializeField] private InventorySlot selectedSlot;

    [SerializeField]
    private Transform handTransform;

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

    public void AddItem(Item item)
    {

        //Already has that type of item, then increase the count
        foreach (InventorySlot slot in slots)
        {
            if (!slot.isSlotEmpty() && slot.GetItem().Type == item.Type)
            {
                slot.AddItem(item.count);

                item.GetComponent<Collider>().enabled = false;
                item.gameObject.SetActive(false);
                item.transform.SetParent(this.handTransform);
                item.gameObject.transform.localPosition = Vector3.zero;
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
                slot.SetItem(item);

                item.GetComponent<Collider>().enabled = false;
                item.gameObject.SetActive(false);
                item.transform.SetParent(this.handTransform);
                item.gameObject.transform.localPosition = Vector3.zero;
                break;
            }
        }

        if (!hasEmptySlot)
        {
            print("DO NOT HAVE EMPTHY SLOT");
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
            Item item = other.GetComponent<Item>();


            AddItem(item);
            ShowInventory();
        }
    }

    public void HoldItem(Item item)
    {
        item.gameObject.SetActive(true);
        item.gameObject.GetComponent<Rigidbody>().isKinematic = true;
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
