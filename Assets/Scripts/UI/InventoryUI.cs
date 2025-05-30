using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI inventorySlotPrefab;
    [SerializeField] private Transform slotsContent;
    [SerializeField] private DescriptionPanel descriptionPanel;
    [SerializeField] private Image draggingImage;

    private List<InventorySlotUI> slots = new();
    private InventorySlotUI selectedSlot;
    private InventorySlotUI currentHoverSlot;

    private Item draggingItem;
    private CanvasGroup canvasGroup;
    private bool draging;
    

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        draggingImage.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Inventory.OnSlotsInitialized += InitializeUI;
    }

    private void OnDisable()
    {
        Inventory.OnSlotsInitialized -= InitializeUI;
    }

    private void InitializeUI()
    {
        CreateSlotUI();
    }

    private void CreateSlotUI()
    {
        var inventorySlots = Inventory.Instance.GetSlots();

        var index = 0;
        foreach (var slot in inventorySlots)
        {
            var slotUI = Instantiate(inventorySlotPrefab, slotsContent);
            slotUI.OnSelected += OnSlotSelected;
            slotUI.OnHovered += OnHovered;
            slotUI.OnUnhovered += OnUnhovered;
            slotUI.OnStartedDrag += OnStartedDrag;
            slotUI.OnEndedDrag += OnEndedDrag;
            slotUI.OnUpdateDrag += OnUpdateDrag;

            slotUI.name = "slot " + index;

            slotUI.SetSlot(slot);
            this.slots.Add(slotUI);

            index++;
        }
    }

    private void OnUpdateDrag(InventorySlotUI obj)
    {
        draggingImage.rectTransform.position = Vector3.Lerp(draggingImage.rectTransform.position, Input.mousePosition, 0.1f);
        //draggingImage.rectTransform.sha
    }

    private void OnEndedDrag(InventorySlotUI slot)
    {
        draging = false;

        Vector2 mousePosition = Input.mousePosition;
        InventorySlotUI hitSlot = GetSlotUnderPosition(mousePosition);

        if (hitSlot != null)
        {
            if (hitSlot.IsEmpty is false)
                slot.SetItem(hitSlot.Item);

            hitSlot.SetItem(draggingItem);
        }
        else
            slot.Slot.SetItem(draggingItem);

        draggingImage.gameObject.SetActive(false);
        draggingImage.sprite = null;

        draggingItem = null;
        slot.EndDrag();
    }

    private InventorySlotUI GetSlotUnderPosition(Vector2 screenPosition)
    {
        // Use GraphicRaycaster to detect UI elements under the mouse
        GraphicRaycaster raycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        // Check if any of the hit objects are inventory slots
        foreach (RaycastResult result in results)
        {
            InventorySlotUI slotUI = result.gameObject.GetComponent<InventorySlotUI>();
            if (slotUI != null && slots.Contains(slotUI))
            {
                return slotUI;
            }
        }

        return null;
    }

    private void OnStartedDrag(InventorySlotUI slot)
    {
        draggingItem = slot.Item;
        slot.SetItem(null);

        draging = true;

        if (selectedSlot)
            selectedSlot.Unselect();
        if (currentHoverSlot)
            currentHoverSlot.Unhover();

        draggingImage.gameObject.SetActive(true);
        draggingImage.sprite = draggingItem.display;
        draggingImage.preserveAspect = true;
        draggingImage.rectTransform.position = Input.mousePosition;

        descriptionPanel.Close();
        slot.StartDrag();
    }

    private void OnUnhovered(InventorySlotUI slot)
    {
        if (draging) return;

        slot.Unhover();
        if (currentHoverSlot == slot)
        {
            currentHoverSlot = null;
            descriptionPanel.Close();
        }
    }

    private void OnHovered(InventorySlotUI slot)
    {
        if (draging) return;

        if (currentHoverSlot != null)
            currentHoverSlot.Unhover();

        currentHoverSlot = slot;
        currentHoverSlot.Hover();

        if (slot.IsEmpty is false)
        {
            descriptionPanel.Open(slot.Item.name, slot.Item.description);
        }
    }

    private void OnSlotSelected(InventorySlotUI slot)
    {
        if (draging) return;

        if (selectedSlot != null)
            selectedSlot.Unselect();

        selectedSlot = slot;
        selectedSlot.Select();
    }

    private void Clear()
    {
        foreach (var slot in slots)
            Destroy(slot.gameObject);

        slots.Clear();
    }

    public void Open()
    {
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    public void Close()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0;
    }
}
