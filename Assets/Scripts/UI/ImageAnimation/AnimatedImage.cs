using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Image))]
public class AnimatedImage : MonoBehaviour
{
    [SerializeField]
    [AddSubclass(typeof(ImageAnimationHandler))]
    private ImageAnimationHandler handler;
    [SerializeField]
    private bool handleActivation;
    
    private Image image;
   
    private void Awake()
    {
        image = GetComponent<Image>();
        if (handleActivation)
            image.enabled = false;
    }

    private void Start()
    {
        handler.OnStart += OnStart;
        handler.OnEnd += OnEnd;
    }

    private void OnEnd()
    {
        if (handleActivation)
            image.enabled = false;
    }

    private void OnStart()
    {
        if (handleActivation)
            image.enabled = true;
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
