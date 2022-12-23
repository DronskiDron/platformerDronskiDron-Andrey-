using UnityEngine;

namespace General.Components
{
    public class DestroyObjectComponent : MonoBehaviour
    {
        [SerializeField] private GameObject _objectToDestroy;


        public void DestroyObject()
        {
            Destroy(_objectToDestroy);
        }
    }
}
