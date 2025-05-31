using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour, ILoadable
{
    public static Inventory Instance;

    [SerializeField] private int slotAmount = 20;
    [SerializeField] private ItemsDatabase database;

    private List<Slot> slots = new();
    private SlotInfo[] slotsInfo;

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
        bool hasSave = InitializeSavedInfo();

        for (int i = 0; i < slotAmount; i++)
        {
            var slot = new Slot();

            if (hasSave && slotsInfo[i].IsEmpty is false)
            {
                Item item = database.itens.Find(item => item.name == slotsInfo[i].itemName);

                if (item != null)
                    slot.SetItem(item);
            }

            slot.OnItemChanged += OnItemChanged;
            slots.Add(slot);
        }

        OnSlotsInitialized?.Invoke();
    }

    private bool InitializeSavedInfo()
    {
        string jsonSave = SaveSystem.LoadFrom(this);

        bool hasSave = string.IsNullOrEmpty(jsonSave) is false;
        slotsInfo = new SlotInfo[slotAmount];
        if (hasSave)
            slotsInfo = JsonConvert.DeserializeObject<SlotInfo[]>(jsonSave);

        return hasSave;
    }

    private void OnItemChanged(Slot slot)
    {
        int index = slots.IndexOf(slot);
        slotsInfo[index].SetInfo(slot);

        SaveSystem.Save(this);
    }

    public void AddItem(Item item)
    {
        if (slots.Any(s => s.IsEmpty()) is false) return;

        var slot = slots.First(s => s.IsEmpty());
        slot.SetItem(item);
    }

    public void RemoveItem(Item item)
    {
        Slot slot = slots.Find(s => s.Item == item);
        slot.SetItem(null);
    }

    public List<Slot> GetSlots()
    {
        return slots;
    }

    public string GetID()
    {
        return "Inventory";
    }

    public string GetSaveInfo()
    {
        if (slotsInfo.Length <= 0)
            CreateSlotInfo();

        return JsonConvert.SerializeObject(slotsInfo);
    }

    private void CreateSlotInfo()
    {
        slotsInfo = new SlotInfo[slots.Count];

        for (int i = 0; i < slots.Count; i++)
        {
            slotsInfo[i] = new SlotInfo()
            {
                itemName = slots[i].Item?.name,
                IsEmpty = slots[i].IsEmpty()
            };
        }
    }

    [Serializable]
    private struct SlotInfo 
    {
        public string itemName;
        public bool IsEmpty;

        public void SetInfo(Slot slot) 
        {
            this.itemName = slot.Item?.name;
            IsEmpty = slot.IsEmpty();
        }
    }
}
