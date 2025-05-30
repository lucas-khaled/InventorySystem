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
    private bool isHovered;

    public void SetItem(Item item) 
    {
        this.Item = item;
        itemImage.sprite = item.display;
        name = item.name;
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

    public void Hover() 
    {
        isHovered = true;
        //animate
    }

    public void Unhover() 
    {
        isHovered = false;
        //animate
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isSelected is false)
            OnSelected?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Entered. Is Hover: "+isHovered + " - Who's entered: " + eventData.pointerEnter.name);
        if(isHovered is false)
            OnHovered?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Exited. Is Hover: " + isHovered + " - Who's entered: "+eventData.pointerEnter.name);
        //if (!eventData.fullyExited || eventData.pointerEnter == gameObject) return;

        if (isHovered)
            OnUnhovered?.Invoke(this);
    }
}
