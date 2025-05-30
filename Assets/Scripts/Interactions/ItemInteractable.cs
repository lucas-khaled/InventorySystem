using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Item item;

    public void Hover()
    {
        Debug.Log("Hover "+name);
    }

    public void Interact()
    {
        Inventory.Instance.AddItem(item);
    }

    public void UnHover()
    {
        Debug.Log("Unhover "+name);
    }
}
