using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Project.Core;
using Project.Coords;

namespace Project
{
    public class MovingPoint : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_numberLabel;
        [SerializeField] private MovingPointInfo m_pointInfo;

        public MovingPointInfo PointInfo => m_pointInfo;
        public Point PointCoords => m_pointInfo.Coords;
        public Vector3 Position => m_pointInfo.Position.ToVector();

        private void Awake()
        {
            if (!m_numberLabel)
            {
                m_numberLabel = GetComponentInChildren<TMP_Text>();
                
                if (!m_numberLabel)
                {
                    throw new NullReferenceException("Label is null!");
                }
            }
        }

        public void SetPosition(Vector3 position)
        {
            this.transform.localPosition = position;
            m_pointInfo.Position = new Point(position.x, position.y);
        }

        public void SetCoords(Point point)
        {
            m_pointInfo.Coords = point;
            SetLabel($"[{point.X.ToString("f1")};{point.Y.ToString("f1")}]");
        }

        public void SetLabel(string text)
        {
            m_numberLabel.text = text;
        }
    }

    public struct MovingPointInfo
    {
        public Point Position { get; set; }
        public Point Coords { get; set; }
    }
}