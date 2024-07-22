using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NeededItem: MonoBehaviour
{
    [SerializeField] Image itemIcon;
    [SerializeField] TextMeshProUGUI itemName;
    [SerializeField] TextMeshProUGUI count;

    public void SetItem(ItemData data)
    {
        itemIcon.sprite = data.Type.icon;
        itemName.text = data.Type.itemName;
        count.text = data.count.ToString();
    }
}