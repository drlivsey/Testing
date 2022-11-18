using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Project.Core;

namespace Project.UI
{
    public class UITweener : MonoBehaviour
    {
        [SerializeField] private float m_duration = 1f;
        [SerializeField] private CoordinatesTypes m_coordinatesSystem = CoordinatesTypes.Local;
        [SerializeField] private RectTransform m_target;
        [SerializeField] private Vector3 m_defaultPosition;
        [SerializeField] private Vector3 m_targetPosition;

        [Header("Events")]
        [SerializeField] private UnityEvent m_onMoveBegin;
        [SerializeField] private UnityEvent m_onMoveEnd;

        public UnityEvent OnMoveBegin => m_onMoveBegin;
        public UnityEvent OnMoveEnd => m_onMoveEnd;

        private Coroutine m_movingRoutine = null;

        private void Awake()
        {
            if (!m_target)
            {
                m_target = this.transform as RectTransform;
            }
        }

        private void OnDisable()
        {
            if (m_movingRoutine == null) return;
            StopCoroutine(m_movingRoutine);
        }

        public void MoveForward()
        {
            MoveToPoint(m_targetPosition, m_duration);
        }

        public void MoveBackwards()
        {
            MoveToPoint(m_defaultPosition, m_duration);
        }

        public void Reset()
        {
            if (m_coordinatesSystem == CoordinatesTypes.Local) {
                m_target.localPosition = m_defaultPosition;
            }
            else {
                m_target.position = m_defaultPosition;
            }
        }

        private void MoveToPoint(Vector3 targetPosition, float duration)
        {
            var beginPosition = m_coordinatesSystem == CoordinatesTypes.Local ? m_target.localPosition : m_target.position;
            if (m_movingRoutine != null)
            {
                StopCoroutine(m_movingRoutine);
            }
            m_movingRoutine = StartCoroutine(MovingRoutine(beginPosition, targetPosition, duration));
        }

        private IEnumerator MovingRoutine(Vector3 beginPoint, Vector3 endPoint, float duration)
        {
            m_onMoveBegin?.Invoke();

            var waitFor = new WaitForEndOfFrame();
            for (var i = 0f; i <= duration; i += Time.deltaTime)
            {
                if (m_coordinatesSystem == CoordinatesTypes.Local) {
                    m_target.localPosition = Vector3.Lerp(beginPoint, endPoint, i / duration);
                }
                else {
                    m_target.position = Vector3.Lerp(beginPoint, endPoint, i / duration);
                }

                yield return waitFor;
            }

            if (m_coordinatesSystem == CoordinatesTypes.Local) {
                    m_target.localPosition = endPoint;
            }
            else {
                m_target.position = endPoint;
            }

            m_onMoveEnd?.Invoke();
        }
    }
}