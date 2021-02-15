
using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Events;

public class GOElementMouseHandler : MonoBehaviour
{
    public event Action onMouseEnter;
    public event Action onMouseOver;
    public event Action onMouseDown;
    public event Action onMouseDrag;
    public event Action onMouseUp;
    public event Action onMouseExit;

    //Incase you want to use UnityEvents instead. Article to help you decide https://www.jacksondunstan.com/articles/3335
    //public UnityEvent onMouseEnter;
    //public UnityEvent onMouseOver;
    //public UnityEvent onMouseDown;
    //public UnityEvent onMouseDrag;
    //public UnityEvent onMouseUp;
    //public UnityEvent onMouseExit;
    public bool debug_logs;

    ////EXAMPLE LISTENINGS
    //void Start()
    //{
    //    AddListernerUsingCSharEvents();
    ////        OR
    //    AddListenerUsingUnityEvents();
    //}

    ////EXAMPLE add listener using C# Events
    //void AddListernerUsingCSharEvents() { onMouseEnter += DoSomething; }
    //void DoSomething() { Log("I got called onMouseEnter using C# Events"); }

    ////EXAMPLE add listener using Unity Events 
    //void AddListenerUsingUnityEvents() {  onBeginDrag.AddListener(delegate { DoSomething2(); }); }
    //void DoSomething2() { Log("I got called onMouseEnter using Unity Events"); }

    void OnMouseEnter()
    {
        onMouseEnter?.Invoke();
        Log($"Mouse enter  {gameObject.name}");
    }
    void OnMouseOver()
    {
        onMouseOver?.Invoke();
        Log($"Mouse over  {gameObject.name}");
    }
    void OnMouseDown()
    {
        onMouseDown?.Invoke();
        Log($"Mouse down  {gameObject.name}");
    }
    void OnMouseDrag()
    {
        onMouseDrag?.Invoke();
        Log($"Mouse drag  {gameObject.name}");
    }
    void OnMouseUp()
    {
        onMouseUp?.Invoke();
        Log($"Mouse up  {gameObject.name}");
    }
    void OnMouseExit()
    {
        onMouseExit?.Invoke();
        Log($"Mouse  exit  {gameObject.name}");
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
