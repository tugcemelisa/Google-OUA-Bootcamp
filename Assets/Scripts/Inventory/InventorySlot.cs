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

    ItemMonoBehaviour item;
    [HideInInspector] public int count = 0;

    [SerializeField] KeyCode keyCode;
    [SerializeField] TextMeshProUGUI slotKeycodeText;

    public KeyCode KeyCode { get => keyCode; }

    private void Start()
    {
        slotKeycodeText.text = keyCode.ToString();
    }

    public void SetItem(ItemMonoBehaviour itemMonoBehaviour)
    {
        if (itemMonoBehaviour == null)
        {
            this.itemNameText.text = "";
            this.icon.sprite = null;
            this.count = 0;
            countText.text = count.ToString();
            PunchScale(icon.transform);
            return;
        }

        this.item = itemMonoBehaviour;

        this.itemNameText.text = itemMonoBehaviour.ItemData.Type.itemName;
        this.icon.sprite = itemMonoBehaviour.ItemData.Type.icon;
        this.count = itemMonoBehaviour.ItemData.count;

        countText.text = count.ToString();
        PunchScale(icon.transform);
    }

    public void AddItem(int count)
    {
        this.count += count;
        countText.text = this.count.ToString();
        PunchScale(countText.transform);
    }

    public void UpdateItem()
    {
        if (item.ItemData.count < 1)
        {
            item = null;

        }

        SetItem(item);
    }

    public void PunchScale(Transform tr)
    {
        tr.transform.localScale = Vector3.one;
        tr.DOPunchScale(Vector3.one * 3, .5f).OnComplete(() =>
        {
            tr.transform.localScale = Vector3.one;
        });
    }

    public bool isSlotEmpty() { return (item == null); }


    public ItemMonoBehaviour GetItem()
    {
        return this.item;
    }

    public void Select()
    {
        if (!isSlotEmpty() && item.ItemData.Type.isHoldable)
            InventoryManager.Instance.HoldItemAnimation(item);
        else InventoryManager.Instance.RemoveItemAnimation();

        this.selectedIcon.SetActive(true);
    }

    internal void Deselect()
    {
        if (item != null)
            item.gameObject.SetActive(false);
        this.selectedIcon.SetActive(false);
    }
}
