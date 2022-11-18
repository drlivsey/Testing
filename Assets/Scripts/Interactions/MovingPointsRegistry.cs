using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public class MovingPointsRegistry : MonoBehaviour
    {
        private List<MovingPoint> m_movingPoints;

        public void AddPoint(MovingPoint point)
        {
            if (m_movingPoints == null) m_movingPoints = new List<MovingPoint>();

            m_movingPoints.Add(point);
        }

        public void RemovePoint(MovingPoint point)
        {
            if (m_movingPoints == null) return;
            if (point == null || !m_movingPoints.Contains(point)) return;

            m_movingPoints.Remove(point);
        }

        public void RemovePointAt(int index)
        {
            if (m_movingPoints == null) return;
            if (index < 0 || index >= m_movingPoints.Count) return;

            m_movingPoints.RemoveAt(index);
        }

        public void Clear()
        {
            if (m_movingPoints == null) return;
            for (var i = 0; i < m_movingPoints.Count; i++)
            {
                Destroy(m_movingPoints[i].gameObject);
            }
            m_movingPoints.Clear();
        }

        public MovingPoint GetPointAt(int index)
        {
            if (m_movingPoints == null) return null;
            if (index < 0 || index >= m_movingPoints.Count) return null;

            return m_movingPoints[index];
        }

        public IEnumerable<MovingPoint> GetPoints()
        {
            return m_movingPoints;
        }

        public int GetPointsCount()
        {
            if (m_movingPoints == null) return 0;

            return m_movingPoints.Count;
        }
    }
}