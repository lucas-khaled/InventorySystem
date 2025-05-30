using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    private List<Item> itens = new();

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
        itens.Add(item);
    }

    public void RemoveItem(Item item) 
    {
        itens.Remove(item);
    }
}
