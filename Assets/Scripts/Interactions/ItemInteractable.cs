using UnityEngine;
using UnityEngine.Events;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private Item item;

    public UnityEvent<bool> OnHover;
    public UnityEvent OnInteract;

    public void Hover()
    {
        OnHover?.Invoke(true);
    }

    public void Interact()
    {
        Inventory.Instance.AddItem(item);
        OnInteract?.Invoke();

        Destroy(gameObject);
    }

    public void UnHover()
    {
        OnHover?.Invoke(false);
    }
}
