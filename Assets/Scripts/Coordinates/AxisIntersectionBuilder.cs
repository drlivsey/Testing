using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Core;

namespace Project.Coords
{
    [RequireComponent(typeof(LineRenderer))]
    public class AxisIntersectionBuilder : MonoBehaviour
    {
        [SerializeField] private CoordinatesPlaneType m_planeType;
        [SerializeField] private CoordinatePlane m_coordinatePlane;
        [SerializeField] private LineRenderer m_intersectionsRenderer;
        [SerializeField] private IntersectionLabel m_intersectionLabelPrefab;
        [SerializeField] private Transform m_dot;

        private Coroutine m_drawingRoutine = null;
        private IntersectionLabel m_oxAxisLabel;
        private IntersectionLabel m_oyAxisLabel;

        private void Awake()
        {
            if (!m_intersectionsRenderer)
            {
                m_intersectionsRenderer = GetComponent<LineRenderer>();
            }

            if (m_coordinatePlane == null)
            {
                throw new NullReferenceException("Coordinate plane is not asigned!");
            }

            if (m_intersectionLabelPrefab == null)
            {
                throw new NullReferenceException("Intersection prefab is not asigned!");
            }

            if (m_dot == null)
            {
                throw new NullReferenceException("Dot transform is not asigned!");
            }
        }

        private void OnEnable()
        {
            m_coordinatePlane.OnAxisRepainted += Redraw;
        }

        private void OnDisable()
        {
            m_coordinatePlane.OnAxisRepainted -= Redraw;
        }

        public void BeginDrawing()
        {
            StopDrawing();

            if (!m_oxAxisLabel && !m_oyAxisLabel)
            {
                m_oxAxisLabel = Instantiate<IntersectionLabel>(m_intersectionLabelPrefab, this.transform);
                m_oyAxisLabel = Instantiate<IntersectionLabel>(m_intersectionLabelPrefab, this.transform);
            }

            m_drawingRoutine = StartCoroutine(DrawingCoroutine());
        }

        public void StopDrawing()
        {
            if (m_drawingRoutine != null)
            {
                StopCoroutine(m_drawingRoutine);
            }
        }

        public void ResetAll()
        {
            for (var i = 0; i < m_intersectionsRenderer.positionCount; i++)
            {
                m_intersectionsRenderer.SetPosition(i, Vector3.zero);
            }
            Destroy(m_oxAxisLabel.gameObject);
            Destroy(m_oyAxisLabel.gameObject);
        }

        public void Redraw()
        {
            if (m_drawingRoutine == null) return;

            var oxAxisLine = m_coordinatePlane.OXAxisLine;
            var oyAxisLine = m_coordinatePlane.OYAxisLine;

            var dotPoint = PositionPoint(m_dot.localPosition);
            var dotCoords = PositionToCoords(m_dot.localPosition);

            var oxParallelLine = Line.GetParallelLine(oxAxisLine, dotPoint);
            var oyParallelLine = Line.GetParallelLine(oyAxisLine, dotPoint);

            var oxIntersectionPoint = Line.GetIntersectionPoint(oxAxisLine, oyParallelLine);
            var oyIntersectionPoint = Line.GetIntersectionPoint(oyAxisLine, oxParallelLine);

            var oxIntersectionPosition = PointToPosition(oxIntersectionPoint);
            var oyIntersectionPosition = PointToPosition(oyIntersectionPoint);

            m_oxAxisLabel.SetLabelTextByFormat(dotCoords.X.ToString("f2"));
            m_oxAxisLabel.SetPosition(oxIntersectionPosition);
            m_oyAxisLabel.SetLabelTextByFormat(dotCoords.Y.ToString("f2"));
            m_oyAxisLabel.SetPosition(oyIntersectionPosition);

            m_intersectionsRenderer.SetPosition(0, oxIntersectionPosition);
            m_intersectionsRenderer.SetPosition(1, m_dot.localPosition);
            m_intersectionsRenderer.SetPosition(2, oyIntersectionPosition);
        }

        private IEnumerator DrawingCoroutine()
        {
            var waitFor = new WaitForEndOfFrame();
            while (true) 
            {
                Redraw();
                yield return waitFor;
            }
        }

        private Point PositionPoint(Vector3 position)
        {
            switch (m_planeType)
            {
                case CoordinatesPlaneType.XOY:
                    return new Point(position.x, position.y);
                case CoordinatesPlaneType.XOZ:
                    return new Point(position.x, position.z);
                case CoordinatesPlaneType.YOZ:
                    return new Point(position.y, position.z);
                default: return new Point(0f, 0f);
            }
        }

        private Point PositionToCoords(Vector3 position)
        {
            switch (m_planeType)
            {
                case CoordinatesPlaneType.XOY:
                    return m_coordinatePlane.PositionToCoords(new Vector3(position.x, position.y));
                case CoordinatesPlaneType.XOZ:
                    return m_coordinatePlane.PositionToCoords(new Vector3(position.x, position.z));
                case CoordinatesPlaneType.YOZ:
                    return m_coordinatePlane.PositionToCoords(new Vector3(position.y, position.z));
                default: return new Point(0f, 0f);
            }
        }

        private Vector3 PointToPosition(Point positionPoint)
        {
            switch (m_planeType)
            {
                case CoordinatesPlaneType.XOY:
                    return new Vector3(positionPoint.X, positionPoint.Y, 0f);
                case CoordinatesPlaneType.XOZ:
                    return new Vector3(positionPoint.X, 0f, positionPoint.Y);
                case CoordinatesPlaneType.YOZ:
                    return new Vector3(0f, positionPoint.X, positionPoint.Y);
                default: return positionPoint.ToVector();
            }
        }
    }
}