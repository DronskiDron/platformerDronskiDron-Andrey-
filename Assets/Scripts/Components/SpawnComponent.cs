using UnityEngine;

namespace General.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _isPrefabFlipX;

        private Transform _transform;


        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }


        private void Start()
        {
            FlipXParticle();
        }


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


        private void FlipXParticle()
        {
            if (_isPrefabFlipX)
            {
                _transform.localScale = new Vector3(_transform.lossyScale.x * -1, _transform.localScale.y, _transform.localScale.z);
            }
        }
    }
}
