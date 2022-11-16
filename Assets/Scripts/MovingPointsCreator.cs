using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

using Project.Coords;
using Project.Core;

namespace Project
{
    public class MovingPointsCreator : MonoBehaviour
    {
        [SerializeField] private MovingPointsRegistry m_registry;
        [SerializeField] private CoordinatePlane m_coordinateSystem;
        [SerializeField] private MovingPoint m_pointPrefab;

        public void CreateMovingPoint(MovingPointInfo pointInfo)
        {
            CreateMovingPoint(pointInfo.Position.ToVector(), pointInfo.Coords);
        }

        public void CreateMovingPoint(Vector3 position, Point coords)
        {
            var point = Instantiate<MovingPoint>(m_pointPrefab, transform);
            
            point.SetPosition(position);
            point.SetCoords(coords);

            m_registry.AddPoint(point);
        }

        public void CreateMovingPoint(PointerEventData eventData)
        {
            var position = (this.transform as RectTransform).InverseTransformPoint(eventData.pointerCurrentRaycast.worldPosition);
            var coords = m_coordinateSystem.PositionToCoords(position);

            CreateMovingPoint(position, coords);
        }

        public void CreateSingleMovingPoint(PointerEventData eventData)
        {
            var position = (this.transform as RectTransform).InverseTransformPoint(eventData.pointerCurrentRaycast.worldPosition);
            var coords = m_coordinateSystem.PositionToCoords(position);

            m_registry.Clear();
            CreateMovingPoint(position, coords);
        }
    }
}