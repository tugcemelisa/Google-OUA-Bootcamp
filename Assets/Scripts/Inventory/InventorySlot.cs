using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI countText;

    [SerializeField] TextMeshProUGUI itemNameText;

    [SerializeField] GameObject selectedIcon;

    Item item;
    int count = 0;

    [SerializeField] KeyCode keyCode;
    [SerializeField] TextMeshProUGUI slotKeycodeText;

    public KeyCode KeyCode { get => keyCode; }

    private void Start()
    {
        slotKeycodeText.text = keyCode.ToString();
    }

    public void SetItem(Item item)
    {

        this.item = item;

        this.itemNameText.text = item.Type.itemName;
        this.icon.sprite = item.Type.icon;
        this.count = item.count;

        countText.text = count.ToString();
        PunchScale(icon.transform);
    }

    public void AddItem(int count)
    {
        this.count += count;
        countText.text = this.count.ToString();
        PunchScale(countText.transform);
    }

    public void PunchScale(Transform tr)
    {
        tr.DOPunchScale(Vector3.one * 3, .5f);
    }

    public bool isSlotEmpty() { return (item == null); }


    public Item GetItem()
    {
        return this.item;
    }

    public void Select()
    {
        if (!isSlotEmpty() && item.Type.isHoldable)
            InventoryManager.Instance.HoldItem(item);

        this.selectedIcon.SetActive(true);
    }

    internal void Deselect()
    {
        if (item != null)
            item.gameObject.SetActive(false);
        this.selectedIcon.SetActive(false);
    }
}
