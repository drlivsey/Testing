using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace ARPresentation.Tweeners
{
    public class TweensParallelizer : Tweener
    {
        [SerializeField] private float m_additionalDelay = 0f;
        [SerializeField] private Tweener[] m_tweensArray;

        [Header("Events")]
        [SerializeField] private UnityEvent m_onTweensBegin;
        [SerializeField] private UnityEvent m_onTweensEnd;

        public override float Duration
        {
            get 
            {
                if (m_tweensArray == null || m_tweensArray.Length == 0)
                    return 0f;
                
                return m_tweensArray.Max(x => x.Duration + x.Delay);
            }
        }
        public override float Delay => m_additionalDelay;

        public override UnityEvent OnTweenBegin => m_onTweensBegin;
        public override UnityEvent OnTweenEnd => m_onTweensEnd;

        public override void TweenForward()
        {
            if (m_tweensArray == null)
            {
                throw new NullReferenceException("Tweens array in empty!");
            }

            StartCoroutine(PlayParallelForward());
        }

        public override void TweenBackwards()
        {
            if (m_tweensArray == null)
            {
                throw new NullReferenceException("Tweens array in empty!");
            }

            StartCoroutine(PlayParallelBackwards());
        }

        private IEnumerator PlayParallelForward()
        {
            yield return new WaitForSeconds(m_additionalDelay);

            m_onTweensBegin?.Invoke();
            
            foreach (var tweener in m_tweensArray)
            {
                tweener.TweenForward();
            }

            yield return new WaitForSeconds(this.Duration);

            m_onTweensEnd?.Invoke();
        }

        private IEnumerator PlayParallelBackwards()
        {
            yield return new WaitForSeconds(m_additionalDelay);

            m_onTweensBegin?.Invoke();
            
            foreach (var tweener in m_tweensArray.Reverse())
            {
                tweener.TweenBackwards();
            }

            yield return new WaitForSeconds(this.Duration);

            m_onTweensEnd?.Invoke();
        }
    }
}