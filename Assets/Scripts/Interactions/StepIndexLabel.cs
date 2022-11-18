using System;
using UnityEngine;

using Project.UI;

namespace Project
{
    public class StepIndexLabel : MonoBehaviour
    {
        [SerializeField] private DotPath m_dotPath;
        [SerializeField] private UITextSetter m_textSetter;
        [SerializeField] private UITweener m_textTweener;
        [SerializeField] private UITextOpacityTweener m_opacityTweener;

        private void Awake()
        {
            if (!m_dotPath)
            {
                throw new NullReferenceException("Dot path is not asigned!");
            }

            if (!m_textSetter)
            {
                throw new NullReferenceException("Text setter is not asigned!");
            }

            if (!m_textSetter)
            {
                throw new NullReferenceException("Text tweener is not asigned!");
            }

            if (!m_textSetter)
            {
                throw new NullReferenceException("Opacity tweener is not asigned!");
            }
        }

        private void OnEnable()
        {
            m_dotPath.OnMoveBegin.AddListener(ShowLabel);
            m_dotPath.OnStepBegin.AddListener(ShowLabel);
        }

        private void OnDisable()
        {
            m_dotPath.OnMoveBegin.RemoveListener(ShowLabel);
            m_dotPath.OnStepBegin.RemoveListener(ShowLabel);
        }

        private void ShowLabel()
        {
            m_textTweener.Reset();
            m_opacityTweener.Reset();
            m_textSetter.SetText((m_dotPath.CurrentPointIndex + 1).ToString());
            m_textTweener.MoveForward();
            m_opacityTweener.MoveForward();
        }
    }
}