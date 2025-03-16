using System;  // <-- Added for Action<>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] private Image itemImage;  // Fixed: Added type (Image)
    [SerializeField] private TMP_Text quantityTxt;
    //[SerializeField] private Image image;

    public event Action<UIInventoryItem> OnInventoryItemClicked;
    public event Action<UIInventoryItem> OnItemDroppedOn;
    public event Action<UIInventoryItem> OnItemBeingDrag;
    public event Action<UIInventoryItem> OnItemEndDrag;
    public event Action<UIInventoryItem> OnRightMouseBtnClick;
    private bool empty = true;

    public void Awake()
    {
        ResetData();
        Deselect();
    }

    public void ResetData()
    {
        this.itemImage.gameObject.SetActive(false);
        empty = true;
    }
    public void Deselect()
    {

    }
    public void SetData(Sprite sprite, int quantity)
    {
        this.itemImage.gameObject.SetActive(true);
        this.itemImage.sprite = sprite;
        this.quantityTxt.text = quantity.ToString(); // Better way to convert int to string
        empty = false;
    }


    public void Select(){

    }

    public void OnBeginDrag(){
        if (empty)
        return;
        OnItemBeingDrag?.Invoke(this);
    }
    public void OnDrop(){
        OnItemDroppedOn?.Invoke(this);
    }
}