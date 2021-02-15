
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.EventSystems;
namespace Mouse_Tool
{
    public class UIElementPointerHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        //FOR THIS SCRIPT TO WORK
        //Needs EventSystem in scene. Right click in scene hierarchy -> UI -> EventSystem
        //This script needs to be attached to the UI Element you wish to be triggering this script.
        //The UI Element needs its option Raycast Target turned on.

        public event Action onPointerEnter;
        public event Action onPointerExit;
        public event Action onPointerDown;
        public event Action onPointerUp;
        public event Action onPointerClick;

        //Incase you want to use UnityEvents instead. Article to help you decide https://www.jacksondunstan.com/articles/3335
        //public UnityEvent onPointerEnter;
        //public UnityEvent onPointerExit;
        //public UnityEvent onPointerDown;
        //public UnityEvent onPointerUp;
        //public UnityEvent onPointerClick;

        public bool debug_logs;

        ////EXAMPLE LISTENINGS
        //void Start()
        //{
        //    AddListernerUsingCSharEvents();
        ////        OR
        //    AddListenerUsingUnityEvents();
        //}
        ////EXAMPLE add listener using C# Events
        //void AddListernerUsingCSharEvents()  { onPointerEnter += DoSomething; }
        //void DoSomething()  { Log("I got called on pointer enter using C# Events"); } 

        ////EXAMPLE add listener using Unity Events 
        //void AddListenerUsingUnityEvents() {  onPointerEnter.AddListener(delegate { DoSomething2(); }); }
        //void DoSomething2() { Log("I got called on pointer enter using Unity Events"); }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onPointerEnter?.Invoke();
            Log($"pointer enter  {gameObject.name}");
        }
        public void OnPointerExit(PointerEventData eventData)
        {
            onPointerExit?.Invoke();
            Log($"pointer exit  {gameObject.name}");
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            onPointerDown?.Invoke();
            Log($"pointer down  {gameObject.name}");
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            onPointerUp?.Invoke();
            Log($"pointer up  {gameObject.name}");
        }
        public void OnPointerClick(PointerEventData eventData)
        {
            onPointerClick?.Invoke();
            Log($"pointer click  {gameObject.name}");
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