using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [SerializeField] private int slotAmount = 20;

    private List<Slot> slots = new();

    private void Awake()
    {
        if(Instance != null) 
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        for(int i = 0; i < slotAmount; i++) 
            slots.Add(new Slot());
    }

    public void AddItem(Item item) 
    {
        if (slots.Any(s => s.IsEmpty()) is false) return;

        var slot = slots.First(s => s.IsEmpty());
        slot.item = item;
    }

    public void RemoveItem(Item item) 
    {
        Slot slot = slots.Find(s => s.item == item);
        slot.item = null;
    }

    public List<Slot> GetSlots() 
    {
        return slots;
    }
}
