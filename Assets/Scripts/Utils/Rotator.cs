using DG.Tweening;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] private float turnsTime;
    [SerializeField] private bool playOnAwake = true;
    [SerializeField] private Vector3 turns = Vector3.up;

    private Sequence sequence;

    private void Awake()
    {
        if (playOnAwake)
            Play();
    }

    public void Play() 
    {
        sequence = DOTween.Sequence();

        sequence.Append(transform.DORotate(turns * 360, turnsTime, RotateMode.FastBeyond360).SetEase(Ease.Unset))
            .SetLoops(-1);
    }

    public void Stop() 
    {
        sequence.Kill();
    }

    private void OnDestroy()
    {
        Stop();
    }
}
