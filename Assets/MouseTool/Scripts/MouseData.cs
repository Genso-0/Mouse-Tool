using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Mouse_Tool
{
    public class MouseData
    {
        public MouseButtonData left;
        public MouseButtonData right;

        public Ray mouseRay;
        public bool detectedSomething;
        public RaycastHit hit;
        public RaycastHit[] hits;
    }
    public struct MouseButtonData
    {
        public float mostRecentClickTime;
        public ButtonFrameState frameState;
        public Vector3 positionAtClick;
        public enum ButtonFrameState
        {
            Inactive,
            Clicked,
            Dragging,
        }
    }
}
