using System;
using UnityEngine;

public class Slot
{
    private Item item;

    // Action that gets invoked whenever the item is modified
    public Action<Slot> OnItemChanged;

    // Encapsulated item property
    public Item Item
    {
        get => item;
        set
        {
            item = value;
            // Invoke the action whenever the item is modified
            OnItemChanged?.Invoke(this);
        }
    }

    public bool IsEmpty() => Item == null;
}
