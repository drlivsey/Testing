using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.UI
{
    public abstract class BaseUITweener : MonoBehaviour
    {
        public abstract void MoveForward();
        public abstract void MoveBackwards();
        public abstract void Reset();
    }
}