using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName;
    [TextArea] public string description;
    public Sprite itemSprite;
    public int stackCount;
}

