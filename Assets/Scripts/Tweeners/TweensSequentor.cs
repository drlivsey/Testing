using System;
using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace ARPresentation.Tweeners
{
    public class TweensSequentor : Tweener
    {
        [SerializeField] private float m_additionalDelay = 0f;
        [SerializeField] private Tweener[] m_tweensSequence;
        
        [Header("Events")]
        [SerializeField] private UnityEvent m_onSequenceBegin;
        [SerializeField] private UnityEvent m_onSequenceEnd;

        public override float Duration
        {
            get 
            {
                if (m_tweensSequence == null || m_tweensSequence.Length == 0)
                    return 0f;
                
                return m_tweensSequence.Sum(x => x.Duration + x.Delay);
            }
        }
        public override float Delay => m_additionalDelay;

        public override UnityEvent OnTweenBegin => m_onSequenceBegin;
        public override UnityEvent OnTweenEnd => m_onSequenceEnd;

        public override void TweenForward()
        {
            if (m_tweensSequence == null)
            {
                throw new NullReferenceException("Tweens sequence in empty!");
            }

            StartCoroutine(PlaySequenceForward());
        }

        public override void TweenBackwards()
        {
            if (m_tweensSequence == null)
            {
                throw new NullReferenceException("Tweens sequence in empty!");
            }

            StartCoroutine(PlaySequenceBackwards());
        }

        private IEnumerator PlaySequenceForward()
        {
            yield return new WaitForSeconds(m_additionalDelay);

            m_onSequenceBegin?.Invoke();

            foreach (var tweener in m_tweensSequence)
            {
                tweener.TweenForward();
                yield return new WaitForSeconds(tweener.Delay + tweener.Duration);
            }

            m_onSequenceEnd?.Invoke();
        }

        private IEnumerator PlaySequenceBackwards()
        {
            yield return new WaitForSeconds(m_additionalDelay);

            m_onSequenceBegin?.Invoke();

            foreach (var tweener in m_tweensSequence.Reverse())
            {
                tweener.TweenBackwards();
                yield return new WaitForSeconds(tweener.Delay + tweener.Duration);
            }

            m_onSequenceEnd?.Invoke();
        }
    }
}