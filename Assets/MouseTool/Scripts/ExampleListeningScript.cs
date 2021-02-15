
using System.Collections;
using UnityEngine;
namespace Mouse_Tool
{
    public class ExampleListeningScript : MonoBehaviour
    {
        void Start()
        {
            var mouseTool = MouseTool.Instance;
            mouseTool.onMouseDown_Left += DoSomethingOnMouseLeftDown;
            mouseTool.onMouseUp_Left += DoSomethingOnMouseLeftUp;

            mouseTool.onMouseDown_Right += DoSomethingOnMouseRightDown;
            mouseTool.onMouseUp_Right += DoSomethingOnMouseRightUp;

            mouseTool.onMouseDrag_Left += DoSomethingOnMouseLeftDrag;
            mouseTool.onMouseDrag_Right += DoSomethingOnMouseRightDrag;
        }
        void DoSomethingOnMouseLeftDown()
        {
            Debug.Log("I listened in on mouse Left down");
        }
        void DoSomethingOnMouseLeftUp()
        {
            Debug.Log("I listened in on mouse Left up");
        }
        void DoSomethingOnMouseRightDown()
        {
            Debug.Log("I listened in on mouse Right down");
        }
        void DoSomethingOnMouseRightUp()
        {
            Debug.Log("I listened in on mouse Right up");
        }
        void DoSomethingOnMouseLeftDrag()
        {
            Debug.Log("I listened in on mouse Left drag");
        }
        void DoSomethingOnMouseRightDrag()
        {
            Debug.Log("I listened in on mouse Right drag");
        }
    }
}