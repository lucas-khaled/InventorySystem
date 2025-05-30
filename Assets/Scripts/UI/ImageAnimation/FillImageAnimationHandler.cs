using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class FillImageAnimationHandler : ImageAnimationHandler
{
    [SerializeField] private float duration = 1f;
    [SerializeField] private float interval = 1f;
    [SerializeField] private LoopType loopType = LoopType.Restart;

    private Sequence sequence;
    private Image image;

    public override void Animate(Image image)
    {
        this.image = image;

        image.type = Image.Type.Filled;
        image.fillAmount = 0;
        sequence = DOTween.Sequence();

        sequence.Append(image.DOFillAmount(1, duration))
                .AppendInterval(interval)
                .Append(image.DOFillAmount(0, duration))
                .SetLoops(-1, loopType);

        sequence.Play();

        OnStart?.Invoke();
    }

    public override void Stop()
    {
        sequence.Kill();

        sequence = DOTween.Sequence();
        sequence.Append(image.DOFillAmount(0, 0.2f)).OnComplete(() => OnEnd?.Invoke());
    }
}
