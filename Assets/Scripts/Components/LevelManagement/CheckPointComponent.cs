using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.Events;

namespace General.Components.LevelManagement
{
    [RequireComponent(typeof(SpawnComponent))]
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private SpawnComponent _playerSpawner;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;
        [SerializeField] private bool _theLastCheckpointOfTheScene = false;

        public string Id { get => _id; set => _id = value; }
        public bool WasChecked { get => _wasChecked; private set => _wasChecked = value; }

        private GameSession _session;
        private bool _wasChecked = false;


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            if (_session.IsChecked(_id))
                _setChecked?.Invoke();
            else
                _setUnchecked?.Invoke();
        }


        public void Check()
        {
            _session.SetChecked(_id);
            _setChecked?.Invoke();
            InformSession();
        }


        public void SpawnPlayer()
        {
            _playerSpawner.Spawn();
        }


        private void InformSession()
        {
            WasChecked = true;
            if (_theLastCheckpointOfTheScene)
                _session.SetThatLevelWasFinished();
            _session.StoreCheckpoints();
        }
    }
}
