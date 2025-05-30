using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private AnimatedImage selectedImage;

    public Action<InventorySlot> OnSelected;
    public Action<InventorySlot> OnHovered;
    public Action<InventorySlot> OnUnhovered;
    
    public Item Item { get; private set; }

    private bool isSelected;

    public void SetItem(Item item) 
    {
        this.Item = item;
        itemImage.sprite = item.display;
    }

    public void Select() 
    {
        isSelected = true;
        selectedImage.Play();
    }

    public void Unselect()
    {
        isSelected = false;
        selectedImage.Stop();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        if(isSelected is false)
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
