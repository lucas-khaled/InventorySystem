using UnityEngine;

[CreateAssetMenu(fileName = "Consumable", menuName = "Scriptable Objects/Consumable")]
public class Consumable : Item
{
    public void Consume() 
    {
        Debug.Log("Consuming "+name);
    }
}
