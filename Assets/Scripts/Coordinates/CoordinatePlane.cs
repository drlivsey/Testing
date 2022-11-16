using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Project.Core;

namespace Project.Coords
{
    public class CoordinatePlane : MonoBehaviour
    {
        [SerializeField] private Axis m_axisOX;
        [SerializeField] private Axis m_axisOY;

        public event UnityAction OnAxisRepainted;

        private void OnEnable()
        {
            m_axisOX.OnAxisRepainted += () => OnAxisRepainted?.Invoke();
            m_axisOY.OnAxisRepainted += () => OnAxisRepainted?.Invoke();
        }

        private void OnDisable()
        {
            m_axisOX.OnAxisRepainted -= () => OnAxisRepainted?.Invoke();
            m_axisOY.OnAxisRepainted -= () => OnAxisRepainted?.Invoke();
        }

        public Point PositionToCoords(Vector3 position)
        {
            var positionPoint = new Point(position.x, position.y);
            var oxLine = m_axisOX.AxisLine;
            var oyLine = m_axisOY.AxisLine;

            var oxPointLine = Line.GetParallelLine(oxLine, positionPoint);
            var oyPointLine = Line.GetParallelLine(oyLine, positionPoint);

            var oxIntersectionPoint = Line.GetIntersectionPoint(oxLine, oyPointLine);
            var oyIntersectionPoint = Line.GetIntersectionPoint(oyLine, oxPointLine);
            
            return new Point(m_axisOX.CoordToValue(oxIntersectionPoint), m_axisOY.CoordToValue(oyIntersectionPoint));
        }

        public Vector3 CoordsToPosition(Point coords)
        {
            return Vector3.zero; //new Vector3(m_axisOX.CoordToPosition(coords.X), m_axisOY.CoordToPosition(coords.Y), 0f);
        }
    }
}