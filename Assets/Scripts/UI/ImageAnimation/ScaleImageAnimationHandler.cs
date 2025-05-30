using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ScaleImageAnimationHandler : ImageAnimationHandler
{
    [SerializeField] private Vector3 toScale = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float interval = 1f;
    [SerializeField] private float duration;
    [SerializeField] private Ease easeIn;
    [SerializeField] private Ease easeOut;
    [SerializeField] private LoopType loopType;

    private Sequence sequence;
    private Vector3 initialScale;
    private Image image;

    public override void Animate(Image image)
    {
        OnStart?.Invoke();
        
        this.image = image;
        sequence = DOTween.Sequence();
        initialScale = image.rectTransform.localScale;

        sequence.Append(image.rectTransform.DOScale(toScale, duration).SetEase(easeIn))
            .AppendInterval(interval)
            .Append(image.rectTransform.DOScale(initialScale, duration))
            .SetLoops(-1, loopType);

    }

    public override void Stop()
    {
        sequence.Kill();

        sequence = DOTween.Sequence();
        sequence.Append(image.rectTransform.DOScale(initialScale, 0.2f)).OnComplete(() => OnEnd?.Invoke());
    }
}
