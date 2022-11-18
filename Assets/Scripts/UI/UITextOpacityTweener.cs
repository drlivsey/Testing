using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

namespace Project.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class UITextOpacityTweener : BaseUITweener
    {
        [SerializeField] private TMP_Text m_textElement;
        [SerializeField] private float m_duration = 1f;
        [SerializeField, Range(0f, 1f)] private float m_beginOpacity = 0f;
        [SerializeField, Range(0f, 1f)] private float m_endOpacity = 1f;

        [Header("Events")]
        [SerializeField] private UnityEvent m_onMoveBegin;
        [SerializeField] private UnityEvent m_onMoveEnd;
        public UnityEvent OnMoveBegin => m_onMoveBegin;
        public UnityEvent OnMoveEnd => m_onMoveEnd;

        private Coroutine m_movingRoutine = null;

        private void Awake()
        {
            if (!m_textElement)
            {
                m_textElement = GetComponent<TMP_Text>();
            }
        }

        private void OnDisable()
        {
            if (m_movingRoutine == null) return;
            StopCoroutine(m_movingRoutine);
        }

        public override void MoveForward()
        {
            MoveToPoint(m_endOpacity, m_duration);
        }

        public override void MoveBackwards()
        {
            MoveToPoint(m_beginOpacity, m_duration);
        }

        public override void Reset()
        {
            var textColor = m_textElement.color;
            m_textElement.color = new Color(textColor.r, textColor.g, textColor.b, m_beginOpacity);
        }

        private void MoveToPoint(float endPoint, float duration)
        {
            if (m_movingRoutine != null)
            {
                StopCoroutine(m_movingRoutine);
            }
            var beginPoint = m_textElement.color.a;
            m_movingRoutine = StartCoroutine(MovingRoutine(beginPoint, endPoint, duration));
        }

        private IEnumerator MovingRoutine(float beginPoint, float endPoint, float duration)
        {
            var waitFor = new WaitForEndOfFrame();
            var textColor = m_textElement.color;

            m_onMoveBegin?.Invoke();
            for (var i = 0f; i <= duration; i += Time.deltaTime)
            {
                var alpha = Mathf.Lerp(beginPoint, endPoint, i / duration);
                m_textElement.color = new Color(textColor.r, textColor.g, textColor.b, alpha);

                yield return waitFor;
            }

            m_textElement.color = new Color(textColor.r, textColor.g, textColor.b, endPoint);
            m_onMoveEnd?.Invoke();
        }
    }
}