
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

namespace Mouse_Tool
{
    [System.Serializable]
    public class MouseTool_Debug
    {
        [Tooltip("Shows current state of MouseTool.MouseTracking(). Read only")] public bool MouseTrackingIsActive;
        [Tooltip("Toggle 2: State of debug")] public bool debug_text;
        [Tooltip("Toggle 3: State of debug ray from camera to wherever the ray is heading")] public bool debug_ray;
        [Tooltip("Toggle 4: State of debug logs in the console")] public bool debug_logs;
        [Tooltip("Toggle 5: Points of contact made by ray on colliders")] public bool debug_rayTouchPoints;
        Text description;
        #region debug
        public void ToggleDebugText(Transform parent)
        {
            SetScreenTextActiveState(parent, !debug_text);
            debug_text = !debug_text;
        }
        public void ToggleDebugLogs()
        {
            debug_logs = !debug_logs;
        }
        public void ToggleDebugRayCast()
        {
            debug_ray = !debug_ray;
        }
        public void ToggleDebugTouchPoints()
        {
            debug_rayTouchPoints = !debug_rayTouchPoints;
        }
        public void SetScreenTextActiveState(Transform parent, bool debugText)
        {
            var canvas = parent.GetChild(0);
            canvas.gameObject.SetActive(debugText);
        }
        public void InitText(Text text)
        {
            description = text;
        }
        public void CheckDefines()
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
            if (!contains)
                if (debug_logs || debug_ray || debug_rayTouchPoints)
                    UnityEngine.Debug.Log("Debug functionality requires for you " +
                        "to have set a define symbol \"LOG\" in Project Settings -> Player -> Scripting Define Symbols. If you do not then the debug methods will not compile.");
        }
        [Conditional("LOG")]
        public void FillInMouseDescriptionText(MouseData mouseData)
        {
            if (debug_text)
            {
                if (mouseData.detectedSomething)
                {
                    description.text = $"For debug options:\n" +
                                       $"LeftShift + 2: Debug text : {debug_text} \n" +
                                       $"LeftShift + 3: Ray cast : {debug_ray}\n" +
                                       $"LeftShift + 4: Console Logs : {debug_logs}\n" +
                                       $"Left button state: {mouseData.left.frameState}\n" +
                                       $"Right button state: {mouseData.right.frameState}\n" +
                                       $"Detected Colliders: {(mouseData.hits != null ? mouseData.hits.Length: 1)}\n" +
                                       $"Nearest: {mouseData.hit.collider.gameObject.name}\n" +
                                       $"Point: {mouseData.hit.point}\n" +
                                       $"Normal: {mouseData.hit.normal}\n" +
                                       $"BarycentricCoordinate: {mouseData.hit.barycentricCoordinate}\n" +
                                       $"Distance from ray origin: {mouseData.hit.distance}\n" +
                                       $"Triangle index: {mouseData.hit.triangleIndex}\n" +
                                       $"TextureCoor1: {mouseData.hit.textureCoord}\n" +
                                       $"TextureCoor2: {mouseData.hit.textureCoord2}\n" +
                                       $"Rigidbody: {mouseData.hit.rigidbody}\n" +
                                       $"ArticulationBody: {mouseData.hit.articulationBody}\n" +
                                       $"LightmapCoord: {mouseData.hit.lightmapCoord}";
                }
                else
                    description.text = $"For debug options:\n" +
                                       $"LeftShift + 2: Debug text : {debug_text}\n" +
                                       $"LeftShift + 3: Ray cast : {debug_ray}\n" +
                                       $"LeftShift + 4: Console Logs : {debug_logs}\n" +
                                       $"Left button state: {mouseData.left.frameState}\n" +
                                       $"Right button state: {mouseData.right.frameState}\n" +
                                       $"Detected: null";
            }
        }
        [Conditional("LOG")]
        public void DrawRay(Vector3 start, Vector3 direction)
        {
            if (debug_ray)
                UnityEngine.Debug.DrawRay(start, direction);
        }
        [Conditional("LOG")]
        public void Log(string message)
        {
            if (debug_logs)
                UnityEngine.Debug.Log(message);
        }
        [Conditional("LOG")]
        public void LogError(string message)
        {
            if (debug_logs)
                UnityEngine.Debug.LogError(message);
        }
        [Conditional("LOG")]
        public void DrawTouchPoints(MouseData mouseData, MouseTool_Settings.RayFireType raycastType)
        {
            if (debug_rayTouchPoints && mouseData.detectedSomething)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawSphere(mouseData.hit.point, 0.1f);
                if (raycastType == MouseTool_Settings.RayFireType.All)
                {
                    Gizmos.color = Color.white;
                    for (int i = 0; i < mouseData.hits.Length; i++)
                        Gizmos.DrawSphere(mouseData.hits[i].point, 0.08f);
                }

            }
        }
       
        #endregion
    }
}
