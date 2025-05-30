using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private List<Item> items = new();

    private void Awake()
    {
        if(Instance != null) 
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void AddItem(Item item) 
    {
        items.Add(item);
    }

    public void RemoveItem(Item item) 
    {
        items.Remove(item);
    }

    public List<Item> GetItems() 
    {
        return items;
    }
}
