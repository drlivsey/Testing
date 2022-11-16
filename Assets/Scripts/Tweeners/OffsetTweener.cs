using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

namespace ARPresentation.Tweeners
{
    public class OffsetTweener : BaseTweener
    {
        [SerializeField] private float m_xAxisOffset;
        [SerializeField] private float m_yAxisOffset;
        [SerializeField] private float m_zAxisOffset;

        [Header("Events")]
        [SerializeField] private UnityEvent m_onTweenBegin;
        [SerializeField] private UnityEvent m_onTweenEnd;

        public override UnityEvent OnTweenBegin => m_onTweenBegin;
        public override UnityEvent OnTweenEnd => m_onTweenEnd;

        public override void TweenBackwards()
        {
            var negativeOffset = new Vector3(-m_xAxisOffset, -m_yAxisOffset, -m_zAxisOffset);
            TweenToPoint(negativeOffset);
        }

        public override void TweenForward()
        {
            var positiveOffset = new Vector3(m_xAxisOffset, m_yAxisOffset, m_zAxisOffset);
            TweenToPoint(positiveOffset);
        }

        protected override void TweenLocal(Transform target, Vector3 offset, float duration, float delay)
        {
            var offsetedPosition = target.localPosition + offset;
            target.DOLocalMove(offsetedPosition, duration)
                .SetDelay(delay)
                .OnStart(() => m_onTweenBegin?.Invoke())
                .OnComplete(() => m_onTweenEnd?.Invoke());
        }

        protected override void TweenGlobal(Transform target, Vector3 offset, float duration, float delay)
        {
            var offsetedPosition = target.position + offset;
            target.DOMove(offsetedPosition, duration)
                .SetDelay(delay)
                .OnStart(() => m_onTweenBegin?.Invoke())
                .OnComplete(() => m_onTweenEnd?.Invoke());
        }
    }
}