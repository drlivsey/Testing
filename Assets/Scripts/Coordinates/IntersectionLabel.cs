using System;
using UnityEngine;
using TMPro;

using Project.UI;

namespace Project.Coords
{
    public class IntersectionLabel : MonoBehaviour
    {
        [SerializeField] private UITextSetter m_intersectionCoordLabel;

        private void Awake()
        {
            if (m_intersectionCoordLabel == null)
            {
                throw new NullReferenceException("Label text element is not asigned!");
            }
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.localPosition = position;
        }

        public void SetLabelText(string text)
        {
            m_intersectionCoordLabel.SetText(text);
        }

        public void SetLabelTextByFormat(string text)
        {
            m_intersectionCoordLabel.SetTextByFormat(text);
        }
    }
}