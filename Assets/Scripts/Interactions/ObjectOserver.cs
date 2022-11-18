using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class ObjectOserver : MonoBehaviour
    {
        [SerializeField] private float m_maxDelta = 1f;
        [SerializeField] private Transform m_trackingObject;
        [SerializeField] private Transform m_trackedObject;
        [SerializeField] private Vector3 m_trackingOffset;

        private Coroutine m_observingRoutine = null;

        private void OnDisable()
        {
            EndObserving();
        }

        public void BeginObserving()
        {
            EndObserving();
            m_observingRoutine = StartCoroutine(ObservTarget());
        }

        public void EndObserving()
        {
            if (m_observingRoutine != null)
            {
                StopCoroutine(m_observingRoutine);
            }
        }

        private IEnumerator ObservTarget()
        {
            var waitFor = new WaitForEndOfFrame();
            while (true)
            {
                var trackingPosition = m_trackedObject.position + m_trackingOffset;
                m_trackingObject.position = Vector3.MoveTowards(m_trackingObject.position, trackingPosition, m_maxDelta);
                yield return waitFor;
            }
        }
    }
}