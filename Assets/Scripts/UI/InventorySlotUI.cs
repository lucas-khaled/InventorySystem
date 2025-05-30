using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private AnimatedImage selectedImage;
    [SerializeField] private AnimatedImage hoveredImage;

    public Action<InventorySlotUI> OnSelected;
    public Action<InventorySlotUI> OnHovered;
    public Action<InventorySlotUI> OnUnhovered;

    public Slot Slot { get; private set; }
    public Item Item => Slot.Item;
    public bool IsEmpty => Slot.IsEmpty();

    private bool isSelected;
    private bool isHovered;

    public void SetSlot(Slot slot)
    {
        if (this.Slot != null)
            this.Slot.OnItemChanged -= OnSlotItemChanged;

        this.Slot = slot;

        if (this.Slot != null)
            this.Slot.OnItemChanged += OnSlotItemChanged;

        SetSlotInfo(slot);
    }

    private void OnSlotItemChanged(Slot slot)
    {
        SetSlotInfo(slot);
    }

    private void SetSlotInfo(Slot slot)
    {
        itemImage.gameObject.SetActive(!IsEmpty);

        if (IsEmpty is false)
            itemImage.sprite = slot.Item.display;
    }

    private void OnDestroy()
    {
        if (Slot != null)
            Slot.OnItemChanged -= OnSlotItemChanged;
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
        hoveredImage.Play();
    }

    public void Unhover()
    {
        isHovered = false;
        hoveredImage.Stop();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Slot.IsEmpty()) return;

        if (isSelected is false)
            OnSelected?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Slot.IsEmpty()) return;

        if (isHovered is false)
            OnHovered?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Slot.IsEmpty()) return;

        if (isHovered)
            OnUnhovered?.Invoke(this);
    }
}
