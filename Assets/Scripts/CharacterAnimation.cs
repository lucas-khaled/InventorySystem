using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private string moveAnimationParamName;

    public void OnMove(InputValue input) 
    {
        Vector2 value = input.Get<Vector2>();

        animator.SetBool(moveAnimationParamName, value.magnitude > 0);
    }
}
