
using System;
using System.Collections;
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
        public float differentiateBetweenSingleClickAndDrag_sensitivity = 0.2f;
        public bool runOnStartup;
        bool run;
        public bool fireRay;
        public bool logs;
        public bool debugText;
        Text description;

        public delegate void OnMouseDown();
        public delegate void OnMouseUp();
        public delegate void OnMouseDrag();

        public event OnMouseDown onMouseDown_Left;
        public event OnMouseDown onMouseDown_Right;

        public event OnMouseUp onMouseUp_Left;
        public event OnMouseUp onMouseUp_Right;

        public event OnMouseDrag onMouseDrag_Left;
        public event OnMouseDrag onMouseDrag_Right;
        void Init()
        {
            if (!initialized)
            {
                initialized = true;
                mouseData = new MouseData();
                description = GetComponentInChildren<Text>();
                if (runOnStartup)
                    BeginMouseTracking();
                else
                    SetScreenTextActiveState(false);
#if UNITY_EDITOR
                CheckDefines();
#endif
            }
        }

        void Start()
        {
            Init();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha1))
                ToggleMouseTracking();
            if (run)
                if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Alpha2))
                    ToggleScreenText();
        }
        void OnApplicationQuit()
        {
            EndMouseTracking();
        }
        IEnumerator MouseTracking()
        {
            SetScreenTextActiveState(debugText);
            while (run)
            {
                if (fireRay)
                    CastRay();
                HandleButton(0, ref mouseData.left);
                HandleButton(1, ref mouseData.right);
                if (debugText)
                    FillInMouseDescriptionText();
                yield return null;
            }
        }
        public void ToggleMouseTracking()
        {
            if (run)
                EndMouseTracking();
            else
                BeginMouseTracking();
        }
        public void ToggleScreenText()
        {
            SetScreenTextActiveState(!debugText);
            debugText = !debugText;
        }
        void SetScreenTextActiveState(bool debugText)
        {
            var canvas = transform.GetChild(0);
            canvas.gameObject.SetActive(debugText);
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
                Log($"Mouse button {button} was clicked");
            }
            else if (Input.GetMouseButton(button) && Time.realtimeSinceStartup - mouseButtonData.mostRecentClickTime > differentiateBetweenSingleClickAndDrag_sensitivity)
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
                Log($"Mouse button {button} is being held down");
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
                Log($"Mouse button {button} was released");
            }

        }
        void FillInMouseDescriptionText()
        {
            if (mouseData.detectedSomething)
                description.text = $"Left button state: {mouseData.left.frameState}\n" +
                                   $"Right button state: {mouseData.right.frameState}\n" +
                                   $"Detected: {mouseData.mouseHit.collider.gameObject.name}\n" +
                                   $"Point: {mouseData.mouseHit.point}\n" +
                                   $"Normal: {mouseData.mouseHit.normal}\n" +
                                   $"BarycentricCoordinate: {mouseData.mouseHit.barycentricCoordinate}\n" +
                                   $"Distance from ray origin: {mouseData.mouseHit.distance}\n" +
                                   $"Triangle index: {mouseData.mouseHit.triangleIndex}\n" +
                                   $"TextureCoor1: {mouseData.mouseHit.textureCoord}\n" +
                                   $"TextureCoor2: {mouseData.mouseHit.textureCoord2}\n" +
                                   $"Rigidbody: {mouseData.mouseHit.rigidbody}\n" +
                                   $"ArticulationBody: {mouseData.mouseHit.articulationBody}\n" +
                                   $"LightmapCoord: {mouseData.mouseHit.lightmapCoord}";
            else
                description.text = $"Left button state: {mouseData.left.frameState}\n" +
                             $"Right button state: {mouseData.right.frameState}\n" +
                             $"Detected: null";
        }
        void CastRay()
        {
            RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
            {
                mouseData.detectedSomething = true;
                mouseData.mouseHit = hit;
                return;
            }
            mouseData.detectedSomething = false;
            mouseData.mouseHit = hit;
            //return info
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
                SetScreenTextActiveState(false);
            }
        }
        public class MouseData
        {
            public MouseButtonData left;
            public MouseButtonData right;

            public bool detectedSomething;
            public RaycastHit mouseHit;
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
        #region logging
        private void CheckDefines()
        {
            UnityEditor.BuildTargetGroup buildTargetGroup = UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup;
            string defines = UnityEditor.PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTargetGroup);
            string[] split_defines = defines.Split(';');
            bool contains = false;
            for (int i = 0; i < split_defines.Length; i++)
            {
                if (split_defines[i] == "LOG")
                    contains = true;
            }
            if (!contains && logs)
                UnityEngine.Debug.Log("The logs functionality requires for you " +
                    "to have set a define symbol \"LOG\" in Project Settings -> Player -> Scripting Define Symbols. If you do not then the logs methods will not compile.");
        }
        [Conditional("LOG")]
        void Log(string message)
        {
            if (logs)
            {
                UnityEngine.Debug.Log(message);
            }
        }
        [Conditional("LOG")]
        void LogError(string message)
        {
            if (logs)
            {
                UnityEngine.Debug.LogError(message);
            }
        }
        #endregion
    }
}