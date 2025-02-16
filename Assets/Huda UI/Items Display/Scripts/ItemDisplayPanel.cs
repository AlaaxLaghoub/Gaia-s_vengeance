using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemDisplayPanel : MonoBehaviour
{
    [Header("Grid Settings")]
    public Vector2 spacing = new Vector2(20, 0); // Space between two items
    public Vector2 itemSize = new Vector2(150, 150);

    [Header("References")]
    public Transform startPoint;
    public GameObject itemPrefab;

    [Header("Runtime Items")]
    public RuntimeInventoryItem firstItem;
    public RuntimeInventoryItem secondItem;

    [Header("Assigned Items (For Initialization)")]
    public InventoryItem firstItemData;  // Assigned in Inspector
    public InventoryItem secondItemData; // Assigned in Inspector

    private void Start()
    {
        InitializeItems(); // Ensure we create runtime instances
        GenerateDisplay();
    }

    private void InitializeItems()
    {
        if (firstItemData != null)
            firstItem = new RuntimeInventoryItem(firstItemData);
        
        if (secondItemData != null)
            secondItem = new RuntimeInventoryItem(secondItemData);
    }

    public void GenerateDisplay()
    {
        foreach (Transform child in startPoint.parent)
        {
            if (child != startPoint)
                Destroy(child.gameObject);
        }

        if (firstItem != null)
            CreateItemSlot(firstItem, 0);
        
        if (secondItem != null)
            CreateItemSlot(secondItem, 1);
    }

    void CreateItemSlot(RuntimeInventoryItem runtimeItem, int index)
    {
        Vector2 position = new Vector2(index * (itemSize.x + spacing.x), 0);
        Vector3 worldPosition = startPoint.position + (Vector3)position;

        GameObject newItem = Instantiate(itemPrefab, startPoint.parent);
        RectTransform rect = newItem.GetComponent<RectTransform>();
        rect.position = worldPosition;

        ItemUI itemUI = newItem.GetComponent<ItemUI>();
        itemUI.SetItem(runtimeItem);
    }
}