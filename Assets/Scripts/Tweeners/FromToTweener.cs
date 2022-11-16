using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARPresentation.Tweeners
{
    public abstract class FromToTweener : BaseTweener
    {
        [SerializeField] private Vector3 m_beginPoint;
        [SerializeField] private Vector3 m_endPoint;

        public override void TweenForward()
        {
            TweenToPoint(m_endPoint);
        }

        public override void TweenBackwards()
        {
            TweenToPoint(m_beginPoint);
        }
    }
}
