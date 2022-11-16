using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace ARPresentation.Tweeners
{
    public class ScaleTweener : FromToTweener
    {
        [Header("Events")]
        [SerializeField] private UnityEvent m_onTweenBegin;
        [SerializeField] private UnityEvent m_onTweenEnd;

        public override UnityEvent OnTweenBegin => m_onTweenBegin;
        public override UnityEvent OnTweenEnd => m_onTweenEnd;

        protected override void TweenLocal(Transform target, Vector3 point, float duration, float delay)
        {
            target.DOScale(point, duration)
                .SetDelay(delay)
                .OnStart(() => m_onTweenBegin?.Invoke())
                .OnComplete(() => m_onTweenEnd?.Invoke());
        }

        protected override void TweenGlobal(Transform target, Vector3 point, float duration, float delay)
        {
            TweenLocal(target, point, duration, delay);
        }
    }
}