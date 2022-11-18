using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

using Project.Coords;
using Project.Core;

namespace Project
{
    public class DotPath : MonoBehaviour
    {
        [SerializeField] private Transform m_target;
        [SerializeField] private float m_speed;
        [SerializeField] private MovingPointsRegistry m_registry;

        [Header("Events")]
        [SerializeField] private UnityEvent m_onMoveBegin;
        [SerializeField] private UnityEvent m_onMoveEnd;
        [SerializeField] private UnityEvent m_onStepBegin;
        [SerializeField] private UnityEvent m_onStepEnd;

        public int CurrentPointIndex => m_currentPointIndex;
        public float CurrentMoveDuration => m_currentMoveDuration;
        public float Speed
        {
            get => m_speed;
            set => m_speed = value;
        }

        public UnityEvent OnMoveBegin => m_onMoveBegin;
        public UnityEvent OnStepBegin => m_onStepBegin;
        public UnityEvent OnStepEnd => m_onStepEnd;
        public UnityEvent OnMoveEnd => m_onMoveEnd;

        private int m_currentPointIndex = -1;
        private float m_currentMoveDuration = 0f;

        private MovingPointInfo m_previousPointInfo = new MovingPointInfo()
        {
            Coords = new Point(0f, 0f),
            Position = new Point(0f, 0f)
        };

        public void MoveToNextStep()
        {
            var pointsCount = m_registry.GetPointsCount();
            if (pointsCount == 0) return;
            if (pointsCount == 1 && m_currentPointIndex >= 0)
            {
                m_currentPointIndex = -1;
            }
            else if (m_currentPointIndex + 1 == pointsCount) return;
            
            var targetPoint = m_registry.GetPointAt(m_currentPointIndex + 1).PointInfo;
            m_currentPointIndex++;

            m_currentMoveDuration = GetMovingDuration(m_previousPointInfo, targetPoint, m_speed);
            StartCoroutine(MoveBetweenPoints(m_previousPointInfo, targetPoint, m_onMoveBegin, m_onMoveEnd));
        }

        public void MoveToPreviousStep()
        {
            if (m_registry.GetPointsCount() == 0 || m_registry.GetPointsCount() == 1) return;
            if (m_currentPointIndex - 1 < 0) return;

            var targetPoint = m_registry.GetPointAt(m_currentPointIndex - 1).PointInfo;
            m_currentPointIndex--;

            m_currentMoveDuration = GetMovingDuration(m_previousPointInfo, targetPoint, m_speed);
            StartCoroutine(MoveBetweenPoints(m_previousPointInfo, targetPoint, m_onMoveBegin, m_onMoveEnd));
        }

        public void MoveFromBeginToEnd()
        {
            var movingPoints = new List<MovingPointInfo>();
            movingPoints.Add(new MovingPointInfo() { Coords = new Point(0f, 0f), Position = new Point(0f, 0f) });
            movingPoints.AddRange(m_registry.GetPoints().Select(x => x.PointInfo));

            m_currentMoveDuration = GetFullMovingDuration(movingPoints);
            StartCoroutine(MoveByPath(movingPoints));
        }

        public void ResetPointPosition()
        {
            m_currentPointIndex = -1;
            m_target.localPosition = Vector3.zero;
        }

        private IEnumerator MoveByPath(IEnumerable<MovingPointInfo> pathPointsInfo)
        {
            m_onMoveBegin?.Invoke();
            for (var i = 1; i < pathPointsInfo.Count(); i++)
            {
                yield return MoveBetweenPoints(pathPointsInfo.ElementAt(i - 1), pathPointsInfo.ElementAt(i), m_onStepBegin, m_onStepEnd);
                m_currentPointIndex++;
            }
            m_onMoveEnd?.Invoke();
        }

        private IEnumerator MoveBetweenPoints(MovingPointInfo currentPoint, MovingPointInfo targetPoint, UnityEvent beginEvent = null, UnityEvent endEvent = null)
        {
            var duration = GetMovingDuration(currentPoint, targetPoint, m_speed);
            var waitFor = new WaitForEndOfFrame();

            beginEvent?.Invoke();
            for (var i = 0f; i <= duration; i += Time.deltaTime)
            {
                m_target.localPosition = Vector3.Lerp(MovingPointToPosition(currentPoint), MovingPointToPosition(targetPoint), i / duration);
                yield return waitFor;
            }
            m_previousPointInfo = targetPoint;
            m_target.localPosition = MovingPointToPosition(targetPoint);
            endEvent?.Invoke();
        }

        protected virtual float GetMovingDuration(MovingPointInfo currentPoint, MovingPointInfo targetPoint, float speed)
        {
            return Point.Distance(currentPoint.Coords, targetPoint.Coords) / speed;
        }

        protected virtual float GetFullMovingDuration(IEnumerable<MovingPointInfo> pathPointsInfo)
        {
            var duration = 0f;
            for (var i = 1; i < pathPointsInfo.Count(); i++)
            {
                duration += GetMovingDuration(pathPointsInfo.ElementAt(i - 1), pathPointsInfo.ElementAt(i), Speed);
            }

            return duration;
        }

        protected virtual Vector3 MovingPointToPosition(MovingPointInfo pointInfo)
        {
            return pointInfo.Position.ToVector();
        }
    }
}