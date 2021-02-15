
using UnityEngine;

namespace Mouse_Tool
{
    [System.Serializable]
    public class MouseTool_Settings
    {
        public float differentiateBetweenSingleClickAndDrag_sensitivity = 0.2f;

        [Tooltip("Calls  BeginMouseTracking() at startup.")] public bool runOnStartup;
        [Tooltip("Toggle: Allows for toggling of MouseTool settings using key inputs LeftShift + 1, 2, 3, 4 etc")] public bool receivingInput;
        [Tooltip("Toggle: UpdateS internal Camera to mouse world position Ray")] public bool trackMousePositionOnScreen;
        [Tooltip("Options: Raycast options. 1) Off : No Raycasts 2) Nearest: Will register nearest collider. 3) All : Returns nearest collider and an array with all registered colliders")]
        public RayFireType raycastType;
        [Tooltip("The layer mask used for raycasting")] public LayerMask rayMask = ~0;
        [Tooltip("Distance for raycast. Only applicable to RayState.All")] public float maxRayDistance = 100f;
        public enum RayFireType
        {
            Off,
            Nearest,
            All
        }
    }
}
