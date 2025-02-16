using UnityEngine;

[System.Serializable]
public class RuntimeInventoryItem
{
    public InventoryItem itemData; // Reference to the ScriptableObject
    public int stackCount; // Separate runtime stack count

    public RuntimeInventoryItem(InventoryItem item)
    {
        itemData = item;
        stackCount = item.stackCount; // Initialize with the original count
    }
}