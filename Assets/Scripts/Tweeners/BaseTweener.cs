using System;
using UnityEngine;

using Project.Core;

namespace ARPresentation.Tweeners
{
    public abstract class BaseTweener : Tweener
    {
        [Header("Options")]
        [SerializeField] private Transform m_tweenTarget = null;
        [SerializeField, Min(0.1f)] private float m_tweenDuration = 1f;
        [SerializeField, Min(0f)] private float m_tweenDelay = 0f;
        [SerializeField] private CoordinatesTypes m_tweenCoordinates;

        public override float Duration => m_tweenDuration;
        public override float Delay => m_tweenDelay;

        private void Awake()
        {
            if (!m_tweenTarget) m_tweenTarget = this.transform;
        }

        protected void TweenToPoint(Vector3 point) 
        {
            if (!m_tweenTarget)
            {
                throw new NullReferenceException("Tween target is null!");
            }

            if (m_tweenCoordinates == CoordinatesTypes.Local)
            {
                TweenLocal(m_tweenTarget, point, m_tweenDuration, m_tweenDelay);
            }
            else if (m_tweenCoordinates == CoordinatesTypes.Global)
            {
                TweenGlobal(m_tweenTarget, point, m_tweenDuration, m_tweenDelay);
            }
        }

        
        protected abstract void TweenLocal(Transform target, Vector3 point, float duration, float delay);
        protected abstract void TweenGlobal(Transform target, Vector3 point, float duration, float delay);
    }
}