using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI inventorySlotPrefab;
    [SerializeField] private Transform slotsContent;
    [SerializeField] private DescriptionPanel descriptionPanel;

    private List<InventorySlotUI> slots = new();
    private InventorySlotUI selectedSlot;
    private InventorySlotUI currentHoverSlot;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
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
            slotUI.name = "slot " + index;

            slotUI.SetSlot(slot);
            this.slots.Add(slotUI);

            index++;
        }
    }

    private void OnUnhovered(InventorySlotUI slot)
    {
        slot.Unhover();
        if (currentHoverSlot == slot)
        {
            currentHoverSlot = null;
            descriptionPanel.Close();
        }
    }

    private void OnHovered(InventorySlotUI slot)
    {
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
