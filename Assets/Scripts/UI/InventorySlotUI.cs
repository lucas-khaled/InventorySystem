using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private AnimatedImage selectedImage;
    [SerializeField] private AnimatedImage hoveredImage;

    public Action<InventorySlotUI> OnSelected;
    public Action<InventorySlotUI> OnHovered;
    public Action<InventorySlotUI> OnUnhovered;
    public Action<InventorySlotUI> OnStartedDrag;
    public Action<InventorySlotUI> OnUpdateDrag;
    public Action<InventorySlotUI> OnEndedDrag;

    public Slot Slot { get; private set; }
    public Item Item => Slot.Item;
    public bool IsEmpty => Slot.IsEmpty();

    private bool isSelected;
    private bool isHovered;
    private bool isDragging;

    public void SetSlot(Slot slot)
    {
        if (this.Slot != null)
            this.Slot.OnItemChanged -= OnSlotItemChanged;

        this.Slot = slot;

        if (this.Slot != null)
            this.Slot.OnItemChanged += OnSlotItemChanged;

        SetSlotInfo(slot);
    }

    public void SetItem(Item item) 
    {
        if (Slot == null) return;

        Slot.SetItem(item);
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

    public void StartDrag() 
    {
        isDragging = true;
    }

    public void EndDrag() 
    {
        isDragging = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsEmpty) return;

        if (isSelected is false)
            OnSelected?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (IsEmpty) return;

        if (isHovered is false)
            OnHovered?.Invoke(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (IsEmpty) return;

        if (isHovered)
            OnUnhovered?.Invoke(this);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsEmpty) return;

        OnStartedDrag.Invoke(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDragging is false) return;

        OnEndedDrag.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDragging is false) return;

        OnUpdateDrag?.Invoke(this);
    }
}
