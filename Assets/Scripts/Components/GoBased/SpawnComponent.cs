using UnityEngine;
using Utils;
using Utils.ObjectPool;

namespace General.Components
{
    public class SpawnComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _isPrefabFlipX;
        [SerializeField] private bool _usePool;

        private Transform _transform;


        private void Awake()
        {
            _transform = GetComponent<Transform>();
            FlipXParticle();
        }


        private void Start()
        {
            FlipXParticle();
        }


        [ContextMenu("Spawn")]
        public void Spawn()
        {
            var targetPosition = _target.position;
            var instance = _usePool
            ? Pool.Instance.Get(_prefab, targetPosition)
            : SpawnUtils.Spawn(_prefab, targetPosition);
            instance.transform.localScale = _target.lossyScale;
        }


        public void SpawnRandom(GameObject prefab, Vector3 position, bool usePool = false)
        {
            var instance = usePool
            ? Pool.Instance.Get(prefab, position)
            : SpawnUtils.Spawn(prefab, position);
        }


        private void FlipXParticle()
        {
            if (_isPrefabFlipX)
            {
                _transform.localScale = new Vector3(_transform.lossyScale.x * -1, _transform.localScale.y, _transform.localScale.z);
            }
        }


        internal void SetPrefab(GameObject prefab)
        {
            _prefab = prefab;
        }
    }
}
