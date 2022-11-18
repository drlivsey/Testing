using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

using Project.Core;

namespace Project.Coords
{
    public class AxisControlPoint : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerMoveHandler
    {
        public event UnityAction onPointCaptured;
        public event UnityAction onPointReleased;
        public event UnityAction onPointMoved;

        public Point PointPosition => new Point(this.transform.localPosition.x, this.transform.localPosition.y);

        private bool m_isCaptured = false;

        public void OnPointerDown(PointerEventData eventData)
        {
            m_isCaptured = true;
            onPointCaptured?.Invoke();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (!m_isCaptured) return;

            this.transform.localPosition = (this.transform.parent as RectTransform).InverseTransformPoint(eventData.pointerCurrentRaycast.worldPosition);
            onPointMoved?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            m_isCaptured = false;
            onPointReleased?.Invoke();
        }
    }
}