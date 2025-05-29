using UnityEngine;

public class ItemInteractable : MonoBehaviour, IInteractable
{
    public void Hover()
    {
        Debug.Log("Hover "+name);
    }

    public void Interact()
    {
        Debug.Log("Interact "+name);
    }

    public void UnHover()
    {
        Debug.Log("Unhover "+name);
    }
}
