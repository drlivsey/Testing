using UnityEngine;
using DG.Tweening;

namespace ARPresentation.Tweeners
{
    public class CyclicRotationTweener : RotationTweener
    {
        protected override void TweenLocal(Transform target, Vector3 point, float duration, float delay)
        {
            target.DOLocalRotate(point, duration)
                .SetDelay(delay)
                .SetLoops(-1, LoopType.Restart)
                .OnStart(() => OnTweenBegin?.Invoke())
                .OnComplete(() => OnTweenEnd?.Invoke());
        }

        protected override void TweenGlobal(Transform target, Vector3 point, float duration, float delay)
        {
            target.DORotate(point, duration)
                .SetDelay(delay)
                .SetLoops(-1, LoopType.Restart)
                .OnStart(() => OnTweenBegin?.Invoke())
                .OnComplete(() => OnTweenEnd?.Invoke());
        }
    }
}