
using UnityEngine;
namespace Mouse_Tool.Extras
{
    public class Spawner : MonoBehaviour
    {
        public GameObject cloneItem;
        public int cloneTimes;
        public Vector3 cloneDirection;
        void Start()
        {
            for (int i = 0; i < cloneTimes; i++)
            {
                var instance = Instantiate(cloneItem, transform);
                instance.transform.position += (cloneDirection * (i + 1));
            }
        }

    }
}