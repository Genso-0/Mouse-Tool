
using Mouse_Tool.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mouse_Tool
{
    public class MouseTool_HitTables
    {
        public MouseTool_HitTables()
        {
            table_InstanceID_to_RaycastHit = new Dictionary<int, RaycastHit>(startTableSize);
            hasBeenHit = new BitArray(startTableSize);
            currentTableSize = startTableSize;
            IDTable = ColliderInstanceIDTable.Instance;
        }
        int startTableSize = 1000;
        int currentTableSize; 
        ColliderInstanceIDTable IDTable;
        BitArray hasBeenHit;
        IDictionary<int, RaycastHit> table_InstanceID_to_RaycastHit; 
        public bool HasBeenHit(int instanceID)
        {
            return hasBeenHit[IDTable.GetIndex(instanceID)];
        }
        public bool TryGetHitInfo(int instanceID, out RaycastHit hit)
        {
            if (table_InstanceID_to_RaycastHit.ContainsKey(instanceID))
            {
                hit = table_InstanceID_to_RaycastHit[instanceID];
                return true;
            }
            hit = new RaycastHit();
            return false;
        }
        public void HandleHits(MouseData mouseData)
        {
            hasBeenHit.SetAll(false);
            if (mouseData.detectedSomething)
            {
                if (mouseData.hits != null)//if this is not null then mouse raycast is set to ALL.
                {
                    for (int i = 0; i < mouseData.hits.Length; i++)
                    {
                        var instanceID = mouseData.hits[i].collider.GetInstanceID();
                        hasBeenHit[IDTable.GetIndex(instanceID)] = true; //set corresponding bit to true
                        table_InstanceID_to_RaycastHit[instanceID] = mouseData.hits[i];
                        if (IDTable.count >= hasBeenHit.Length - 1)
                             GrowTables(ref i); 
                    }
                }
                else
                {
                    var instanceID = mouseData.hit.collider.GetInstanceID();
                    hasBeenHit[IDTable.GetIndex(instanceID)] = true; //set corresponding bit to true
                    table_InstanceID_to_RaycastHit[instanceID] = mouseData.hit; 
                }
            }
        }

        private void GrowTables(ref int index)
        {
            index = 0;
            currentTableSize += startTableSize;
            table_InstanceID_to_RaycastHit = new Dictionary<int, RaycastHit>(currentTableSize);
            hasBeenHit = new BitArray(currentTableSize);
        }

    }
}
