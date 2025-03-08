using UnityEngine;

public class PotionPickup : MonoBehaviour
{
    public InventoryItem potionItem; // Assign potion item in Inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Potion collected: " + potionItem.itemName); // Debug message

            InventoryUI inventory = FindObjectOfType<InventoryUI>();
            if (inventory != null)
            {
                inventory.AddToInventory(potionItem);
            }

            PlayerShooting playerShooting = other.GetComponent<PlayerShooting>();
            if (playerShooting != null)
            {
                if (potionItem.itemName == "Fire Potion")
                {
                    Debug.Log("Fireball unlocked!");
                    playerShooting.UnlockFireball();
                }
                else if (potionItem.itemName == "Ice Potion")
                {
                    Debug.Log("Ice Shard unlocked!");
                    playerShooting.UnlockIceShard();
                }
            }

            Destroy(gameObject);
        }
    }

}
