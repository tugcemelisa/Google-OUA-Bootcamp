
using UnityEngine;

[System.Serializable]
public class ItemMonoBehaviour : MonoBehaviour
{
    [SerializeField] ItemData itemData;

    public ItemData ItemData { get => itemData; set => itemData = value; }

    public bool isSameType(ItemMonoBehaviour item)
    {
        return item.ItemData.Type == ItemData.Type;
    }

    public bool isSameTypeAndEqualCount(ItemMonoBehaviour item)
    {
        return isSameType(item) && item.ItemData.count == ItemData.count;
    }
}
