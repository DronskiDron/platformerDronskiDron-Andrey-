using UnityEngine;

namespace General.Components.LevelManagement
{
    public class LevelBarrierComponent : MonoBehaviour
    {
        [SerializeField] private GameObject[] _barriers;
        [SerializeField] private bool _isActive = false;


        private void Start()
        {
            foreach (var barrier in _barriers)
            {
                barrier.SetActive(false);
            }
            _isActive = _barriers[0].activeSelf;
        }


        [ContextMenu("ToggleBarrier")]
        public void OnToggleBarrier()
        {
            foreach (var barrier in _barriers)
            {
                barrier.SetActive(!barrier.activeSelf);
            }
            _isActive = _barriers[0].activeSelf;
        }


        public void OnActivateBarrier()
        {
            foreach (var barrier in _barriers)
            {
                barrier.SetActive(true);
            }
            _isActive = _barriers[0].activeSelf;
        }


        public void OnDeactivateBarrier()
        {
            foreach (var barrier in _barriers)
            {
                barrier.SetActive(false);
            }
            _isActive = _barriers[0].activeSelf;
        }
    }
}
