using UnityEngine;
using UnityEngine.Events;

namespace ARPresentation.Tweeners
{
    public abstract class Tweener : MonoBehaviour
    {
        public abstract float Duration { get; }
        public abstract float Delay { get; }
        
        public abstract UnityEvent OnTweenBegin { get; }
        public abstract UnityEvent OnTweenEnd { get; }

        public abstract void TweenForward();
        public abstract void TweenBackwards();
    }
}