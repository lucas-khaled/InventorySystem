using System;
using UnityEngine;

public class CharacterInteraction : MonoBehaviour
{
    [SerializeField] private Transform interactionCheckTransform;
    [SerializeField] private float interactionRadius = 10f;
    [SerializeField] private LayerMask interactionMask;

    private IInteractable currentInteractable;

    private void FixedUpdate()
    {
        FindInteractables();
    }

    private void FindInteractables()
    {
        RaycastHit[] allHit = Physics.SphereCastAll(interactionCheckTransform.position, interactionRadius, Vector3.up, interactionMask);

        float distance = float.MaxValue;
        IInteractable newInteractable = null;
        foreach (var hit in allHit)
        {
            var interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable == null) continue;

            if (hit.distance < distance)
            {
                newInteractable = interactable;
                distance = hit.distance;
            }
        }

        HoverInteractable(newInteractable);
    }

    private void HoverInteractable(IInteractable interactable)
    {
        if (currentInteractable == interactable) return;

        if (currentInteractable != null)
            currentInteractable.UnHover();

        currentInteractable = interactable;
        if (interactable == null)
            return;

        currentInteractable.Hover();
    }

    public void OnInteract() 
    {
        if (currentInteractable == null) return;

        currentInteractable.Interact();
    }

    private void OnDrawGizmosSelected()
    {
        if (interactionCheckTransform == null) return;

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(interactionCheckTransform.position, interactionRadius);
    }
}
