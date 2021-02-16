
using UnityEngine;
using UnityEngine.UI;

namespace Mouse_Tool.Extras
{
    class DisplayHitState : MonoBehaviour
    {
        public Text description;
        MouseTool mouseTool;
        int instanceID;
        void Start()
        {
            instanceID = GetComponent<Collider>().GetInstanceID();
            mouseTool = MouseTool.Instance;
        }
        void Update()
        {
            var hasBeenHit = mouseTool.hitTables.HasBeenHit(instanceID);

            if (hasBeenHit)
            {
                mouseTool.hitTables.TryGetHitInfo(instanceID, out RaycastHit hit);
                description.text = $"Object {gameObject.name} with ID {instanceID}\n" +
                    $"Has been hit by raycast : {hasBeenHit}\n" +
                    $"Hit info:\n" +
                    $"Point: {hit.point}\n" +
                    $"Distance from ray origin {hit.distance}";
            }
            else
            {
                description.text = $"Object {gameObject.name} with ID {instanceID}\n" +
                   $"Has been hit by raycast : {hasBeenHit}";
            }
        }
    }
}
