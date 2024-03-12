using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.Events;

namespace General.Components.LevelManagement
{
    public class RestoreStateComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        public string Id => _id;
        public UnityEvent _onRestore;

        private GameSession _session;
        private string _sceneName;


        private void OnValidate()
        {
            _id = gameObject.name;
        }


        private void Start()
        {
            _session = GameSession.Instance;
            _sceneName = GameSession.Instance.GetCurrentSceneName();
            var isDestroyed = _session.ItemStateStorage.RestoreState(_sceneName, Id);
            if (isDestroyed)
                _onRestore?.Invoke();
        }


        public void OnStoreState()
        {
            GameSession.Instance.ItemStateStorage.StoreState(_sceneName, Id);
        }
    }
}
