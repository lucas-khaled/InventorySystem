using UnityEngine;

public class Slot
{
    public Item item;

    public bool IsEmpty() => item == null;
}
