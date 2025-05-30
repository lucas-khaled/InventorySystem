using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class ImageAnimationHandler : MonoBehaviour
{
    public abstract void Animate(Image image);
    public abstract void Stop();
}