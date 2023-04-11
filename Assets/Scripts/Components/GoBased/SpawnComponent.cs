using UnityEngine;
using Utils;

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
            var instance = SpawnUtils.Spawn(_prefab, _target.position);
            instance.transform.localScale = _target.lossyScale;
        }


        public void SpawnRandom(GameObject prefab, Vector3 position)
        {
            var instance = SpawnUtils.Spawn(prefab, position);
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
