using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

using Project.Core;

namespace Project.Coords
{
    public class Axis : MonoBehaviour
    {
        [SerializeField] private AxisType m_axisType;
        [SerializeField] private AxisControlPoint m_controlPoint;
        [SerializeField] private Color m_axisColor = Color.yellow;
        [SerializeField] private float m_valueOnDivision = 1f;
        [SerializeField] private float m_pixelsPerUnit = 10f;
        [SerializeField] private LineRenderer m_axisRenderer;
        [SerializeField] private Division m_divisionPrefab;

        public float AxisAngle => GetAxleAngle();
        public Line AxisLine => GetAxisLine();
        
        public float DivisionValue
        {
            get => m_valueOnDivision;
            set
            {
                m_valueOnDivision = value;
                UpdateDivisions();
            }
        }

        public float DivisionsDensity
        {
            get => m_pixelsPerUnit;
            set 
            {
                m_pixelsPerUnit = value;
                UpdateDivisions();
            }
        }
        
        public event UnityAction OnAxisRepainted;

        private List<Division> m_divisions;
        private Coroutine m_repaintCoroutine;

        private void Awake()
        {
            InitializeComponent();
            InstantiateDivisions();
        }

        private void OnEnable()
        {
            m_controlPoint.onPointCaptured += DestroyDivisions;
            m_controlPoint.onPointReleased += InstantiateDivisions;
            m_controlPoint.onPointMoved += RepaintLine;
        }

        private void OnDisable()
        {
            m_controlPoint.onPointCaptured -= DestroyDivisions;
            m_controlPoint.onPointReleased -= InstantiateDivisions;
            m_controlPoint.onPointMoved -= RepaintLine;
        }

        public Vector3 ValueToCoord(float value)
        {
            return Vector3.zero;
            //return m_pixelsPerUnit * value / m_valueOnDivision;
        }

        public float CoordToValue(Point point)
        {
            var value = Point.Distance(new Point(0f, 0f), point) / m_pixelsPerUnit * m_valueOnDivision;

            if ((m_axisType == AxisType.OY && point.Y < 0f) || (m_axisType == AxisType.OX && point.X < 0f)) 
            {
                value = -value;
            }

            return value;
        }

        public Line GetAxisLine()
        {
            return new Line(new Point(0f, 0f), m_controlPoint.PointPosition);
        }

        public float GetAxleAngle()
        {
            var indentityLine = new Line(new Point(-1f, 0f), new Point(1f, 0f));
            var angle = Line.GetAngleBetween(indentityLine, AxisLine);

            angle = angle > 90f ? angle - 180f : angle;

            return m_controlPoint.PointPosition.Y >= 0f ? angle : -angle;
        }

        private void InitializeComponent()
        {
            m_divisions = new List<Division>();
            InstantiateDivisions();
        }

        private void RepaintLine()
        {
            var boundsLines = GetViewportBoundsLines();
            var axisLine = GetAxisLine();

            Point closestIntersectionPoint = null;
            Point zeroPoint = new Point(0f, 0f);

            for (var i = 0; i < boundsLines.Length; i++)
            {
                if (!Line.Intersect(axisLine, boundsLines[i])) continue;

                var intersectionPoint = Line.GetIntersectionPoint(axisLine, boundsLines[i]);
                if (closestIntersectionPoint == null)
                {
                    closestIntersectionPoint = intersectionPoint;
                    continue;
                }

                if (Point.Distance(zeroPoint, intersectionPoint) < Point.Distance(zeroPoint, closestIntersectionPoint))
                {
                    closestIntersectionPoint = intersectionPoint;
                }
            }

            m_axisRenderer.SetPosition(0, new Vector3(-closestIntersectionPoint.X, -closestIntersectionPoint.Y));
            m_axisRenderer.SetPosition(1, new Vector3(closestIntersectionPoint.X, closestIntersectionPoint.Y));

            OnAxisRepainted?.Invoke();
        }

        private void InstantiateDivisions()
        {
            var beginPoint = m_axisRenderer.GetPosition(0);
            var endPoint = m_axisRenderer.GetPosition(1);

            var axisLength = Vector3.Distance(beginPoint, endPoint);
            var axisAngle = GetAxleAngle();
            var divisionsCount = (int)(axisLength / m_pixelsPerUnit);

            for (var i = -divisionsCount / 2; i <= divisionsCount / 2; i++)
            {
                if (i == 0) continue;

                var division = Instantiate<Division>(m_divisionPrefab, this.transform);
                var divisionPosition = new Vector3(i * m_pixelsPerUnit, 0f, 0f);
                var divisionRotation = new Vector3(0f, 0f, axisAngle);
                var divisionValue = (i * m_valueOnDivision);

                if (axisAngle < 0f && m_axisType == AxisType.OY) 
                {
                    divisionValue = -divisionValue;
                }

                division.SetPosition(divisionPosition.RotatePointAroundPivot(Vector3.zero, divisionRotation));
                division.SetRotation(divisionRotation);
                division.SetLabel(divisionValue.ToString());
                division.SetColor(m_axisColor);
                
                m_divisions.Add(division);
            }
        }

        private void DestroyDivisions()
        {
            if (m_divisions.Count != 0)
            {
                for (var i = 0; i < m_divisions.Count; i++)
                {
                    Destroy(m_divisions[i].gameObject);
                }
                m_divisions.Clear();
            }
        }

        private void UpdateDivisions()
        {
            DestroyDivisions();
            InstantiateDivisions();
        }
        
        private Line[] GetViewportBoundsLines()
        {
            var viewportTransform = this.transform as RectTransform;
            
            var firstPoint = new Point(viewportTransform.rect.xMin, viewportTransform.rect.yMax);
            var secondPoint = new Point(viewportTransform.rect.xMax, viewportTransform.rect.yMax);
            var thirdPoint = new Point(viewportTransform.rect.xMax, viewportTransform.rect.yMin);
            var fourthPoint = new Point(viewportTransform.rect.xMin, viewportTransform.rect.yMin);

            return new Line[] { new Line(firstPoint, secondPoint), new Line(secondPoint, thirdPoint), new Line(thirdPoint, fourthPoint), new Line(fourthPoint, firstPoint) };
        }
    }
}