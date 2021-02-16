
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
namespace Mouse_Tool
{
    public class MouseTool : MonoBehaviour
    {
        #region singleton
        private static MouseTool m_Instance;
        public static MouseTool Instance
        {
            get
            {
                m_Instance = FindObjectOfType<MouseTool>();
                if (m_Instance == null)
                {
                    UnityEngine.Debug.LogError("Instance not in scene!");
                }
                else if (!m_Instance.initialized)
                {
                    m_Instance.Init();
                }
                return m_Instance;
            }
        }
        public bool initialized { get; private set; }
        #endregion
        IEnumerator routine_mouseTracking;
        public MouseData mouseData;
        public MouseTool_Settings settings;
        public MouseTool_Debug debug;
        public MouseTool_HitTables hitTables;
        bool run;
        public event Action onMouseDown_Left;
        public event Action onMouseDown_Right;
        public event Action onMouseUp_Left;
        public event Action onMouseUp_Right;
        public event Action onMouseDrag_Left;
        public event Action onMouseDrag_Right; 
        void Init()
        {
            if (!initialized)
            {
                initialized = true;
                mouseData = new MouseData();
                hitTables = new MouseTool_HitTables();
                debug.InitText(GetComponentInChildren<Text>());
                if (settings.runOnStartup)
                    BeginMouseTracking();
                else
                    debug.SetScreenTextActiveState(transform, false);
#if UNITY_EDITOR
                debug.CheckDefines();
#endif
            }
        }
         
        void Start()
        {
            Init();
        }
        void Update()
        {
            debug.MouseTrackingIsActive = run;
            if (settings.receivingInput && Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    ToggleMouseTracking();
                if (run)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                        debug.ToggleDebugText(transform);
                    if (Input.GetKeyDown(KeyCode.Alpha3))
                        debug.ToggleDebugRayCast();
                    if (Input.GetKeyDown(KeyCode.Alpha4))
                        debug.ToggleDebugLogs();
                    if (Input.GetKeyDown(KeyCode.Alpha5))
                        debug.ToggleDebugTouchPoints();
                }
            }
            else if (settings.receivingInput)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                    ToggleMousePositionTracking();
            }
        }
        IEnumerator MouseTracking()
        {
            debug.SetScreenTextActiveState(transform, debug.debug_text);
            while (run)
            {
                if (settings.trackMousePositionOnScreen)
                    mouseData.mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                HandleRays();
                hitTables.HandleHits(mouseData);
                HandleButton(0, ref mouseData.left);
                HandleButton(1, ref mouseData.right);

                debug.FillInMouseDescriptionText(mouseData);
                yield return null;
            }
        }
        void HandleButton(int button, ref MouseButtonData mouseButtonData)
        {
            if (Input.GetMouseButtonDown(button))
            {
                mouseButtonData.mostRecentClickTime = Time.realtimeSinceStartup;
                mouseButtonData.positionAtClick = Input.mousePosition;
                mouseButtonData.frameState = MouseButtonData.ButtonFrameState.Clicked;
                switch (button)
                {
                    case 0:
                        onMouseDown_Left?.Invoke();
                        break;
                    case 1:
                        onMouseDown_Right?.Invoke();
                        break;
                    default:
                        break;
                }
                debug.Log($"Mouse button {button} was clicked");
            }
            else if (Input.GetMouseButton(button) && Time.realtimeSinceStartup - mouseButtonData.mostRecentClickTime > settings.differentiateBetweenSingleClickAndDrag_sensitivity)
            {
                mouseButtonData.frameState = MouseButtonData.ButtonFrameState.Dragging;
                switch (button)
                {
                    case 0:
                        onMouseDrag_Left?.Invoke();
                        break;
                    case 1:
                        onMouseDrag_Right?.Invoke();
                        break;
                    default:
                        break;
                }
                debug.Log($"Mouse button {button} is being held down");
            }
            if (Input.GetMouseButtonUp(button))
            {
                mouseButtonData.frameState = MouseButtonData.ButtonFrameState.Inactive;
                switch (button)
                {
                    case 0:
                        onMouseUp_Left?.Invoke();
                        break;
                    case 1:
                        onMouseUp_Right?.Invoke();
                        break;
                    default:
                        break;
                }
                debug.Log($"Mouse button {button} was released");
            }
        }
        void HandleRays()
        {
            switch (settings.raycastType)
            {
                case MouseTool_Settings.RayFireType.Off:
                    mouseData.detectedSomething = false;
                    mouseData.hit = new RaycastHit();
                    mouseData.hits = null;
                    return;
                case MouseTool_Settings.RayFireType.Nearest:
                    mouseData.detectedSomething = Physics.Raycast(mouseData.mouseRay, out mouseData.hit, settings.maxRayDistance, settings.rayMask);
                    mouseData.hits = null;
                    break;
                case MouseTool_Settings.RayFireType.All:
                    mouseData.detectedSomething = Physics.Raycast(mouseData.mouseRay, out mouseData.hit, settings.maxRayDistance, settings.rayMask);
                    if (mouseData.detectedSomething)
                        mouseData.hits = Physics.RaycastAll(Camera.main.transform.position, mouseData.mouseRay.direction, settings.maxRayDistance, settings.rayMask);
                    else mouseData.hits = null;
                    break;
            }
            //RaycastHit info
            //collider: The Collider that was hit. 
            //point: The impact point in world space where the ray hit the collider. 
            //normal: The normal of the surface the ray hit.
            //barycentricCoordinate: The barycentric coordinate of the triangle we hit. 
            //distance: The distance from the ray's origin to the impact point.
            //triangleIndex: The index of the triangle that was hit. 
            //textureCoord: The uv texture coordinate at the collision location. 
            //textureCoord2: The secondary uv texture coordinate at the impact point. 
            //transform: The Transform of the rigidbody or collider that was hit.
            //rigidbody: The Rigidbody of the collider that was hit. If the collider is not attached to a rigidbody then it is null.
            //articulationBody: The ArticulationBody of the collider that was hit. If the collider is not attached to an articulation body then it is null.
            //lightmapCoord:  The uv lightmap coordinate at the impact point.
        }

        public void ToggleMouseTracking()
        {
            if (run)
                EndMouseTracking();
            else
                BeginMouseTracking();
        }
        private void ToggleMousePositionTracking()
        {
            settings.trackMousePositionOnScreen = !settings.trackMousePositionOnScreen;
        }

        void BeginMouseTracking()
        {
            routine_mouseTracking = MouseTracking();
            run = true;
            StartCoroutine(routine_mouseTracking);
        }
        void EndMouseTracking()
        {
            if (routine_mouseTracking != null)
            {
                StopCoroutine(routine_mouseTracking);
                routine_mouseTracking = null;
                run = false;
                debug.SetScreenTextActiveState(transform, false);
            }
        }
        void OnApplicationQuit()
        {
            EndMouseTracking();
        }
        void OnDrawGizmos()
        {
            if (mouseData != null)
            {
                debug.DrawTouchPoints(mouseData, settings.raycastType);
                debug.DrawRay(Camera.main.transform.position, mouseData.mouseRay.direction * settings.maxRayDistance);
            }
        }

    }
}