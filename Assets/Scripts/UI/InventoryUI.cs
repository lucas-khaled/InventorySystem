using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventorySlots inventorySlotPrefab;
    [SerializeField] private Transform slotsContent;

    private List<InventorySlots> slots = new();

    public void Open() 
    {
        Clear();
        var items = Inventory.Instance.GetItems();

        foreach (var item in items) 
        {
            var slot = Instantiate(inventorySlotPrefab, slotsContent);
            slot.SetItem(item);
            slots.Add(slot);
        }
    }

    private void Clear() 
    {
        foreach(var slot in slots) 
            Destroy(slot.gameObject);

        slots.Clear();
    }
}
