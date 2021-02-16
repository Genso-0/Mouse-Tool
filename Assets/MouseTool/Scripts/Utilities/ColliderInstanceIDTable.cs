 
using System.Collections.Generic; 

namespace Mouse_Tool.Utilities
{
    public class ColliderInstanceIDTable
    {
        #region singleton
        private static ColliderInstanceIDTable m_Instance;
        public static ColliderInstanceIDTable Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new ColliderInstanceIDTable();
                }
                return m_Instance;
            }
        }
        #endregion
        public ColliderInstanceIDTable()
        {
            table_collider_instanceID_to_index = new Dictionary<int, int>();
        } 
        IDictionary<int, int> table_collider_instanceID_to_index; 
        public int count { get { return table_collider_instanceID_to_index.Count; } }
        public void Add(int collider_instanceID)
        {
            table_collider_instanceID_to_index.Add(collider_instanceID, table_collider_instanceID_to_index.Count);
        }
        public int GetIndex(int collider_instanceID)
        {
            if (!table_collider_instanceID_to_index.ContainsKey(collider_instanceID))
                Add(collider_instanceID);
            return table_collider_instanceID_to_index[collider_instanceID];
        } 
    }
}
