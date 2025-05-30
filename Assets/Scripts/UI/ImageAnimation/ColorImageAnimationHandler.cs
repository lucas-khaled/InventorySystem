using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ColorImageAnimationHandler : ImageAnimationHandler
{
    [SerializeField] private Color toColor;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float interval = 1f;
    [SerializeField] private LoopType loopType = LoopType.Restart;

    private Sequence sequence;
    private Color initialColor;
    private Image image;

    public override void Animate(Image image)
    {
        this.image = image;
        initialColor = image.color;

        image.fillAmount = 1;
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0);

        sequence = DOTween.Sequence();

        sequence.Append(image.DOColor(toColor, duration))
                .AppendInterval(interval)
                .Append(image.DOColor(initialColor, duration))
                .SetLoops(-1, loopType);

        OnStart?.Invoke();
    }

    public override void Stop()
    {
        sequence.Kill();

        sequence = DOTween.Sequence();
        sequence.Append(image.DOColor(initialColor, 0.2f)).OnComplete(() => OnEnd?.Invoke());
    }
}
