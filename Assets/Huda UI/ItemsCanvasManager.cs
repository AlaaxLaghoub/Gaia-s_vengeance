using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemsCanvasManager : MonoBehaviour
{
    [Header("Panels")]
    public GameObject inventoryPanel;
    public ItemDisplayPanel itemDisplayPanel;

    [Header("Key Bindings")]
    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode firstItemKey = KeyCode.K;
    public KeyCode secondItemKey = KeyCode.L;

    private void Update()
    {
        HandleInventoryToggle();
        HandleItemUsage();
    }

    void HandleInventoryToggle()
    {
        if (Input.GetKeyDown(inventoryKey))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

    void HandleItemUsage()
    {
        if (itemDisplayPanel == null) return;

        if (Input.GetKeyDown(firstItemKey))
        {
            UseItem(0);
        }

        if (Input.GetKeyDown(secondItemKey))
        {
            UseItem(1);
        }
    }

    void UseItem(int slotIndex)
    {
        RuntimeInventoryItem runtimeItem = slotIndex == 0 ? itemDisplayPanel.firstItem : itemDisplayPanel.secondItem;

        if (runtimeItem != null)
        {
            runtimeItem.stackCount--;

            if (runtimeItem.stackCount <= 0)
            {
                // Remove item when stack is empty
                if (slotIndex == 0)
                    itemDisplayPanel.firstItem = null;
                else
                    itemDisplayPanel.secondItem = null;
            }

            // Refresh the display panel UI
            itemDisplayPanel.GenerateDisplay();
        }
    }
}