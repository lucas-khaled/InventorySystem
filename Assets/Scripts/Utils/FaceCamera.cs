using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    [SerializeField] private bool inverted;
    [SerializeField][Range(0, 1)] private float lerpAmount = 0.7f;

    private void Update()
    {
        Camera cam = Camera.main;

        var direction = (inverted) ? transform.position - cam.transform.position : cam.transform.position - transform.forward;
        var rotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lerpAmount);
    }
}
