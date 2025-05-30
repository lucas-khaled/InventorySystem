using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlot inventorySlotPrefab;
    [SerializeField] private Transform slotsContent;
    [SerializeField] private DescriptionPanel descriptionPanel;

    private List<InventorySlot> slots = new();
    private InventorySlot selectedSlot;
    private InventorySlot currentHoverSlot;

    public void Open() 
    {
        Clear();
        var items = Inventory.Instance.GetItems();

        foreach (var item in items) 
        {
            var slot = Instantiate(inventorySlotPrefab, slotsContent);
            slot.OnSelected += OnSlotSelected;
            slot.OnHovered += OnHovered;
            slot.OnUnhovered += OnUnhovered;

            slot.SetItem(item);
            slots.Add(slot);
        }
    }

    private void OnUnhovered(InventorySlot slot)
    {
        slot.Unhover();
        if (currentHoverSlot == slot)
        {
            currentHoverSlot = null;
            descriptionPanel.Close();
        }
    }

    private void OnHovered(InventorySlot slot)
    {
        if (currentHoverSlot != null)
            currentHoverSlot.Unhover();

        currentHoverSlot = slot;
        currentHoverSlot.Hover();

        descriptionPanel.Open(slot.Item.name, slot.Item.description);
    }

    private void OnSlotSelected(InventorySlot slot)
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
