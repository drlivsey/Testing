using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Project.Core;

namespace Project
{
    public class DotPath3D : DotPath
    {
        [SerializeField] private CoordinatesPlaneType m_planeType;

        protected override float GetMovingDuration(MovingPointInfo currentPoint, MovingPointInfo targetPoint, float speed)
        {
            return Point.Distance(currentPoint.Coords, targetPoint.Coords) / speed;
        }

        protected override Vector3 MovingPointToPosition(MovingPointInfo pointInfo)
        {
            switch (m_planeType)
            {
                case CoordinatesPlaneType.XOY:
                    return new Vector3(pointInfo.Position.X, pointInfo.Position.Y, 0f);
                case CoordinatesPlaneType.XOZ:
                    return new Vector3(pointInfo.Position.X, 0f, pointInfo.Position.Y);
                case CoordinatesPlaneType.YOZ:
                    return new Vector3(0f, pointInfo.Position.X, pointInfo.Position.Y);
                default: return pointInfo.Position.ToVector();
            }
        }
    }
}