using UnityEngine;

[CreateAssetMenu(fileName ="New Item Type", menuName = "Item/Create New Item Type")]
public class ItemType : ScriptableObject
{
    public int id;
    public string itemName;
    public float value;
    public Sprite icon;
    public bool isHoldable = true;
}
