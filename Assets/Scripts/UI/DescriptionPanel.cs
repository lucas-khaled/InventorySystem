using System.Collections;
using TMPro;
using UnityEngine;

public class DescriptionPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Settings")]
    [SerializeField] private Vector2 mouseOffset;
    [SerializeField] [Range(0,1)] private float positionLerp = 0.5f;

    private Coroutine followRoutine;

    public void Open(string name, string description) 
    {
        gameObject.SetActive(true);

        nameText.text = name;
        descriptionText.text = description;

        followRoutine = StartCoroutine(Follow());
    }

    public void Close() 
    {
        StopCoroutine(followRoutine);
        gameObject.SetActive(false);
    }

    private IEnumerator Follow() 
    {
        while (true) 
        {
            var position = Input.mousePosition;
            var newPos = position + new Vector3(mouseOffset.x, mouseOffset.y, 0);
            transform.position = Vector3.Lerp(transform.position, newPos, positionLerp);

            yield return new WaitForEndOfFrame();
        }
    }
}
