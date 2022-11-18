using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Project.Core;

namespace Project
{
    public class Delay : MonoBehaviour
    {
        [SerializeField] private DelayType m_delayType;
        [SerializeField] private float m_delay;
        [SerializeField] private bool m_startOnEnable = false;

        [Header("Events")]
        [SerializeField] private UnityEvent m_onCountdownBegin;
        [SerializeField] private UnityEvent m_onCountdownEnd;

        private Coroutine m_countdownRoutine = null;

        private void Start()
        {
            if (!m_startOnEnable) return;

            BeginCountdown();
        }

        private void OnDisable()
        {
            StopCountdown();
        }

        public void BeginCountdown()
        {
            StopCountdown();
            StartCoroutine(CountdownCoroutine(m_delay));
        }

        public void BeginCountdown(float delay)
        {
            StopCountdown();
            StartCoroutine(CountdownCoroutine(delay));
        }

        public void StopCountdown()
        {
            if (m_countdownRoutine == null) return;
            StopCoroutine(m_countdownRoutine);
        }

        private IEnumerator CountdownCoroutine(float delay)
        {
            m_onCountdownBegin?.Invoke();

            if (m_delayType == DelayType.Realtime)
                yield return new WaitForSecondsRealtime(delay);
            else 
                yield return new WaitForSeconds(delay);

            m_onCountdownEnd?.Invoke();
        }
    }
}