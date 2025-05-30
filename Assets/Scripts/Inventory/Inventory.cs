using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [SerializeField] private int slotAmount = 20;

    private List<Slot> slots = new();

    public static Action OnSlotsInitialized;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        for (int i = 0; i < slotAmount; i++)
            slots.Add(new Slot());

        OnSlotsInitialized?.Invoke();
    }

    public void AddItem(Item item)
    {
        if (slots.Any(s => s.IsEmpty()) is false) return;

        var slot = slots.First(s => s.IsEmpty());
        slot.Item = item;
    }

    public void RemoveItem(Item item)
    {
        Slot slot = slots.Find(s => s.Item == item);
        slot.Item = null;
    }

    public List<Slot> GetSlots()
    {
        return slots;
    }
}
