using System;
using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.Events;

namespace General.Components.LevelManagement
{
    [RequireComponent(typeof(SpawnComponent))]
    [RequireComponent(typeof(SaveLoadManager))]
    public class CheckPointComponent : MonoBehaviour
    {
        [SerializeField] private string _id;
        [SerializeField] private SpawnComponent _playerSpawner;
        [SerializeField] private RestoreStateComponent _state;
        [SerializeField] private UnityEvent _setChecked;
        [SerializeField] private UnityEvent _setUnchecked;

        public string Id { get => _id; set => _id = value; }
        public bool WasChecked { get => _wasChecked; private set => _wasChecked = value; }

        private static Action OnActivated;
        private static CheckPointComponent _lastActivatedCheckpoint;

        private GameSession _session;
        private bool _wasChecked = false;


        private void Start()
        {
            _session = GameSession.Instance;
            OnActivated += Uncheck;
            
            _setUnchecked?.Invoke();
        }


        public void Check()
        {
            _session.SetChecked(_id);
            _session.GetCurrentSceneManagementInfo().SetActualLevelCheckpoint(_id);
            _session.Loader.SaveData();
            _setChecked?.Invoke();
            _lastActivatedCheckpoint = this;
            OnActivated?.Invoke();
            InformSession();

            if (_state != null)
                _session.ItemStateStorage.StoreState(_session.GetCurrentSceneName(), _state.Id);
        }


        public void SpawnPlayer()
        {
            _playerSpawner.Spawn();
        }


        private void InformSession()
        {
            WasChecked = true;
            _session.StoreAnyCheckpoint(_id);
            _session.LocalSaveSession();
            _session.Loader.SaveData();
        }


        private void Uncheck()
        {
            if (this != _lastActivatedCheckpoint)
                _setUnchecked?.Invoke();
        }


        private void OnDestroy()
        {
            OnActivated += Uncheck;
        }
    }
}
