using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlots : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;

    public Action<InventorySlots> OnSelected;
    public Action<InventorySlots> OnHovered;
    public Action<InventorySlots> OnUnhovered;
    
    public Item Item { get; private set; }

    public void SetItem(Item item) 
    {
        this.Item = item;
        itemImage.sprite = item.display;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelected?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnHovered?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        OnUnhovered?.Invoke(this);
    }
}
