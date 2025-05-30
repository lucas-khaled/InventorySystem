using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlotUI inventorySlotPrefab;
    [SerializeField] private Transform slotsContent;
    [SerializeField] private DescriptionPanel descriptionPanel;

    private List<InventorySlotUI> slots = new();
    private InventorySlotUI selectedSlot;
    private InventorySlotUI currentHoverSlot;

    public void Open() 
    {
        Clear();
        var slots = Inventory.Instance.GetSlots();

        foreach (var slot in slots) 
        {
            var slotUI = Instantiate(inventorySlotPrefab, slotsContent);
            slotUI.OnSelected += OnSlotSelected;
            slotUI.OnHovered += OnHovered;
            slotUI.OnUnhovered += OnUnhovered;

            slotUI.SetSlot(slot);
            this.slots.Add(slotUI);
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

        descriptionPanel.Open(slot.Item.name, slot.Item.description);
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
        foreach(var slot in slots) 
            Destroy(slot.gameObject);

        slots.Clear();
    }
}
