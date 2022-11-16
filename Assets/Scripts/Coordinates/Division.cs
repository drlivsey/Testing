using System;
using UnityEngine;
using TMPro;

namespace Project.Coords
{
    [RequireComponent(typeof(LineRenderer), typeof(TMP_Text))]
    public class Division : MonoBehaviour
    {
        [SerializeField] private LineRenderer m_divisionRenderer;
        [SerializeField] private TMP_Text m_divisionLabel;

        private void Awake()
        {
            if (!m_divisionLabel) 
            {
                m_divisionLabel = this.GetComponent<TMP_Text>();
            }

            if (!m_divisionRenderer)
            {
                m_divisionRenderer = this.GetComponent<LineRenderer>();
            }
        }

        public void SetLabel(string text)
        {
            m_divisionLabel.SetText(text);
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.localPosition = position;
        }

        public void SetRotation(Vector3 eulerAngles)
        {
            this.transform.localEulerAngles = eulerAngles;
        }

        public void SetColor(Color color)
        {
            m_divisionRenderer.startColor = color;
            m_divisionRenderer.endColor = color;

            m_divisionLabel.color = color;
        }
    }
}
