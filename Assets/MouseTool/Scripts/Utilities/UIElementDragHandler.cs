
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace Mouse_Tool
{
    public class UIElementDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        //FOR THIS SCRIPT TO WORK
        //Needs EventSystem in scene. Right click in scene hierarchy -> UI -> EventSystem
        //This script needs to be attached to the UI Element you wish to be triggering this script.
        //The UI Element needs its option Raycast Target turned on.
    
        public event Action onBeginDrag;
        public event Action onDrag;
        public event Action onEndDrag;

        //Incase you want to use UnityEvents instead. Article to help you decide https://www.jacksondunstan.com/articles/3335
        //public UnityEvent onBeginDrag;
        //public UnityEvent onDrag;
        //public UnityEvent onEndDrag; 

        public bool debug_logs;

        ////EXAMPLE LISTENINGS
        //void Start()
        //{
        //    AddListernerUsingCSharEvents();
        ////        OR
        //    AddListenerUsingUnityEvents();
        //}

        ////EXAMPLE add listener using C# Events
        //void AddListernerUsingCSharEvents() { onBeginDrag += DoSomething; }
        //void DoSomething() { Log("I got called on begin drag using C# Events"); }

        ////EXAMPLE add listener using Unity Events 
        //void AddListenerUsingUnityEvents() {  onBeginDrag.AddListener(delegate { DoSomething2(); }); }
        //void DoSomething2() { Log("I got called on begin drag using Unity Events"); }

        public void OnBeginDrag(PointerEventData eventData)
        {
            onBeginDrag?.Invoke();
            Log($"on begin drag {gameObject.name}");
        }
        public void OnDrag(PointerEventData data)
        {
            onDrag?.Invoke();
            Log($"on drag  {gameObject.name}");
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            onEndDrag?.Invoke();
            Log($"on end drag  {gameObject.name}");
        }

        //Will only compile if you have "LOG" in your Scripting Define Symbols under Project Settings/Player
        [Conditional("LOG")]
        void Log(string message)
        {
            if (debug_logs)
            {
                UnityEngine.Debug.Log(message);
            }
        }
    }
}