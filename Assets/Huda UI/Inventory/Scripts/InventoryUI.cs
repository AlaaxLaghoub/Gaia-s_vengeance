using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUI : MonoBehaviour
{
    [Header("Grid Settings")]
    public int rows = 3;
    public int columns = 3;
    public Vector2 spacing = new Vector2(10, 10);
    public Vector2 itemSize = new Vector2(100, 100);

    [Header("References")]
    public RectTransform inventoryPanel;
    public Transform startPoint; // New empty transform for the starting position
    public GameObject itemPrefab; // Prefab for UI items
    public List<InventoryItem> inventoryItems = new List<InventoryItem>();
    private void Start()
    {
        GenerateInventoryUI();
    }

    void GenerateInventoryUI()
    {
        // Clear previous items
        foreach (Transform child in startPoint.parent) // Clears UI items
        {
            if (child != startPoint) // Don't destroy the startPoint itself
                Destroy(child.gameObject);
        }

        for (int i = 0; i < inventoryItems.Count; i++)
        {
            int row = i / columns;
            int col = i % columns;

            // Calculate the new position relative to startPoint
            Vector2 position = new Vector2(col * (itemSize.x + spacing.x), -row * (itemSize.y + spacing.y));
            Vector3 worldPosition = startPoint.position + (Vector3)position;

            GameObject newItem = Instantiate(itemPrefab, startPoint.parent); // Instantiate under the same parent
            RectTransform rect = newItem.GetComponent<RectTransform>();
            rect.position = worldPosition;

            // Populate UI item
            ItemUI itemUI = newItem.GetComponent<ItemUI>();
            itemUI.SetItem(inventoryItems[i]);
        }
    }
    public void AddToInventory(InventoryItem item)
    {
        Debug.Log("Adding to inventory: " + item.itemName);

        foreach (var inventoryItem in inventoryItems)
        {
            if (inventoryItem.itemName == item.itemName)
            {
                Debug.Log("Item already exists in inventory.");
                return;
            }
        }

        inventoryItems.Add(item);
        Debug.Log("Inventory count after adding: " + inventoryItems.Count);
        GenerateInventoryUI();
    }


}