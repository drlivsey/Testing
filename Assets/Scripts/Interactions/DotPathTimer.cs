using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Project
{
    [RequireComponent(typeof(TMP_Text))]
    public class DotPathTimer : MonoBehaviour
    {
        [SerializeField] private DotPath m_targetPath;
        [SerializeField] private TMP_Text m_timerTextField;

        private void Awake()
        {
            if (!m_targetPath)
            {
                throw new NullReferenceException("Path is null!");
            }

            if (!m_timerTextField)
            {
                m_timerTextField = GetComponent<TMP_Text>();
            }
        }

        private void OnEnable()
        {
            m_targetPath?.OnMoveBegin.AddListener(OnPointMoved);
        }

        private void OnDisable()
        {
            m_targetPath?.OnMoveBegin.RemoveListener(OnPointMoved);
        }

        private void OnPointMoved()
        {
            var movingDuration = m_targetPath.CurrentMoveDuration;
            StartCoroutine(TimerCoroutine(movingDuration));
        }

        private IEnumerator TimerCoroutine(float duration)
        {
            var waitFor = new WaitForEndOfFrame();
            for (var i = duration; i >= 0; i -= Time.deltaTime)
            {
                m_timerTextField.text = DurationToTimeFormat(i);
                yield return waitFor;
            }

            m_timerTextField.text = "00:00:00.0";
        }

        private string DurationToTimeFormat(float value)
        {
            var time = TimeSpan.FromSeconds(value);
            return $"{ToTimeFormat(time.Hours)}:{ToTimeFormat(time.Minutes)}:{ToTimeFormat(time.Seconds)}.{time.Milliseconds.ToString("f2")}";
        }

        private string ToTimeFormat(double value)
        {
            if (value < 10.0) return $"0{value}";
            else return $"{value}";
        }
    }
}