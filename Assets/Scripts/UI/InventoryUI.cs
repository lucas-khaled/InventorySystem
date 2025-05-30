using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlot inventorySlotPrefab;
    [SerializeField] private Transform slotsContent;
    [SerializeField] private Image selectedImage;

    private List<InventorySlot> slots = new();
    private InventorySlot selectedSlot;

    public void Open() 
    {
        Clear();
        var items = Inventory.Instance.GetItems();

        foreach (var item in items) 
        {
            var slot = Instantiate(inventorySlotPrefab, slotsContent);
            slot.OnSelected += OnSlotSelected;
            slot.SetItem(item);
            slots.Add(slot);
        }
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
