using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class AnimatedImage : MonoBehaviour
{
    [SerializeField]
    [AddSubclass(typeof(ImageAnimationHandler))]
    private ImageAnimationHandler handler;
    
    private Image image;
    private Sequence sequence;
    
    private bool stopping = false;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Play() 
    {
        handler.Animate(image);
    }

    public void Stop() 
    {
        handler.Stop();  
    }
}
