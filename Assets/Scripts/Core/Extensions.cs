using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project
{
    public static class Extensions
    {
        public static Vector3 RotatePointAroundPivot(this Vector3 point, Vector3 pivot, Vector3 angles) 
        {
            return Quaternion.Euler(angles) * (point - pivot) + pivot;
        }
    }
}