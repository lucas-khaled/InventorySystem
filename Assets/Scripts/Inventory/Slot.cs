using System;
using UnityEngine;

[Serializable]
public class Slot
{
    public Action<Slot> OnItemChanged;

    public Item Item { get; private set; }

    public void SetItem(Item item) 
    {
        Item = item;
        OnItemChanged?.Invoke(this);
    }

    public bool IsEmpty() => Item == null;
}
