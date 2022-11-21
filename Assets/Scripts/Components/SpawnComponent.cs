using UnityEngine;

namespace General.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;


        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var instance = Instantiate(_prefab, _target.position, Quaternion.identity);
            instance.transform.localScale = _target.lossyScale;
        }


        public void SpawnRandom(GameObject prefab, Vector3 position, Quaternion quaternion)
        {
            var instance = Instantiate(prefab, position, quaternion);
        }
    }
}
