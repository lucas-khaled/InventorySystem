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
    [SerializeField] private Button removeButton;

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
        removeButton.onClick.AddListener(RemoveCurrentItem);
    }

    private void OnEnable()
    {
        Inventory.OnSlotsInitialized += InitializeUI;
        removeButton.gameObject.SetActive(false);
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
    }

    private void OnEndedDrag(InventorySlotUI slot)
    {
        draging = false;

        Vector2 mousePosition = Input.mousePosition;

        GetAreaResultType resultType;
        InventorySlotUI hitSlot = GetAreaUnderPosition(mousePosition, out resultType);

        switch (resultType) 
        {
            case GetAreaResultType.None:
                slot.Slot.SetItem(draggingItem);
                break;
            case GetAreaResultType.Slot:
                SwapSlot(slot, hitSlot);
                break;
            case GetAreaResultType.RemoveButton:
                slot.SetItem(null);
                break;
        }

        draggingImage.gameObject.SetActive(false);
        draggingImage.sprite = null;

        removeButton.gameObject.SetActive(false);

        draggingItem = null;
        slot.EndDrag();
    }

    private void SwapSlot(InventorySlotUI slot, InventorySlotUI hitSlot)
    {
        if (hitSlot.IsEmpty is false)
            slot.SetItem(hitSlot.Item);

        hitSlot.SetItem(draggingItem);
    }

    private InventorySlotUI GetAreaUnderPosition(Vector2 screenPosition, out GetAreaResultType type)
    {
        GraphicRaycaster raycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        raycaster.Raycast(pointerData, results);

        foreach (RaycastResult result in results)
        {
            InventorySlotUI slotUI = result.gameObject.GetComponent<InventorySlotUI>();
            if (slotUI != null && slots.Contains(slotUI))
            {
                type = GetAreaResultType.Slot;
                return slotUI;
            }

            if(result.gameObject == removeButton.gameObject)
            {
                type = GetAreaResultType.RemoveButton;
                return null;
            }
        }

        type = GetAreaResultType.None;
        return null;
    }

    private void OnStartedDrag(InventorySlotUI slot)
    {
        draggingItem = slot.Item;
        slot.SetItem(null);

        removeButton.gameObject.SetActive(true);

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

        removeButton.gameObject.SetActive(true);
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

        removeButton.gameObject.SetActive(false);
    }

    private void RemoveCurrentItem()
    {
        if (selectedSlot == null) return;

        selectedSlot.Unselect();
        selectedSlot.SetItem(null);
        selectedSlot = null;
        removeButton.gameObject.SetActive(false);
    }

    private enum GetAreaResultType 
    {
        None,
        Slot,
        RemoveButton
    }
}
