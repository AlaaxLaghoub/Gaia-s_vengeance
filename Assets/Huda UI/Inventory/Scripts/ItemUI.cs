using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemUI : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text stackText;

    public void SetItem(InventoryItem item)
    {
        itemImage.sprite = item.itemSprite;
        stackText.text = item.stackCount > 1 ? item.stackCount.ToString() : "";
    }
    
    public void SetItem(RuntimeInventoryItem runtimeItem)
    {
        if (runtimeItem == null) return;

        itemImage.sprite = runtimeItem.itemData.itemSprite;
        stackText.text = runtimeItem.stackCount > 1 ? runtimeItem.stackCount.ToString() : "";
    }
}