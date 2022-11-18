using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Project.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class UITextSetter : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_textElement;
        [SerializeField] private string m_format;

        private void Awake()
        {
            if (!m_textElement)
            {
                m_textElement = GetComponent<TMP_Text>();
            }
        }

        public void SetText(string text)
        {
            m_textElement.text = text;
        }

        public void SetTextByFormat(string text)
        {
            m_textElement.text = string.Format(m_format, text);
        }

        public void SetTextByFormat(object arg)
        {
            m_textElement.text = string.Format(m_format, arg);
        }

        public void SetTextByFormat(string format, params string[] args)
        {
            m_textElement.text = string.Format(m_format, args);
        }
    }
}