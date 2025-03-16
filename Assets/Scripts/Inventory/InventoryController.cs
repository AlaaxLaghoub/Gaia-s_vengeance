using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage inventoryUI;

    int inventorysize = 10;

    private void Start(){
        inventoryUI.InitializeInventoryUI(inventorysize);
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // Press 'I' to toggle inventory
        {
            if (!inventoryUI.isActiveAndEnabled)
            {
                inventoryUI.Show(); // Ensure 'Show' is properly defined
            }
            else
            {
                inventoryUI.Hide(); // Ensure 'Hide' is properly defined
            }
        }
    }

}
