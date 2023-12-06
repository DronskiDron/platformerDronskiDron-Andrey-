using System;
using System.Collections.Generic;
using System.Linq;
using Creatures.Model.Data.Models;
using Creatures.Model.Data.ScenesManagement;
using General.Components.LevelManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils.Disposables;

namespace Creatures.Model.Data
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private ScenesManagementInfo[] _scenesInfo;

        public PlayerData Data => _data;
        private PlayerData _sessionSave;
        private int _storedSceneIndex;

        private readonly CompositeDisposable _trash = new CompositeDisposable();

        public QuickInventoryModel QuickInventory { get; private set; }
        public BigInventoryModel BigInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }

        private List<string> _checkpoints = new List<string>();


        private void Awake()
        {
            var existsSession = GetExistsSession();
            var currentSceneInfo = FindSceneManagementInfo(GetCurrentSceneName());
            if (existsSession != null)
            {
                existsSession.StartSession(currentSceneInfo.LevelEnterCheckpoint);
                Destroy(gameObject);
            }
            else
            {
                InitModels();
                DontDestroyOnLoad(this);
                StartSession(currentSceneInfo.LevelEnterCheckpoint);
            }
        }


        private void StartSession(string defaultCheckPoint)
        {
            SetChecked(defaultCheckPoint);
            LoadHud();
            SpawnPlayer();
        }


        private void SpawnPlayer()
        {
            var currentSceneName = GetCurrentSceneName();
            var currentSceneInfo = FindSceneManagementInfo(currentSceneName);
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            if (currentSceneInfo.GetSceneStatusFlag() && !IsMoveToUpperIndexScene())
            {
                foreach (var checkPoint in checkpoints)
                {
                    if (checkPoint.Id == FindSceneManagementInfo(GetCurrentSceneName()).LevelExitCheckpoint)
                    {
                        checkPoint.SpawnPlayer();
                        break;
                    }
                }
            }
            else if (currentSceneInfo.GetSceneStatusFlag() && IsMoveToUpperIndexScene())
            {
                foreach (var checkPoint in checkpoints)
                {
                    if (checkPoint.Id == FindSceneManagementInfo(GetCurrentSceneName()).LevelEnterCheckpoint)
                    {
                        checkPoint.SpawnPlayer();
                        break;
                    }
                }
            }
            else
            {
                var lastCheckPoint = _checkpoints.Last();

                foreach (var checkPoint in checkpoints)
                {
                    if (checkPoint.Id == lastCheckPoint)
                    {
                        checkPoint.SpawnPlayer();
                        break;
                    }
                }
            }
        }


        private void InitModels()
        {
            QuickInventory = new QuickInventoryModel(_data);
            _trash.Retain(QuickInventory);

            BigInventory = new BigInventoryModel(_data);
            _trash.Retain(BigInventory);

            PerksModel = new PerksModel(_data);
            _trash.Retain(PerksModel);

            StatsModel = new StatsModel(_data);
            _trash.Retain(StatsModel);
        }


        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }


        private void Start()
        {
            SaveSession();
        }


        private GameSession GetExistsSession()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this)
                    return gameSession;
            }
            return null;
        }


        public void SaveSession()
        {
            _sessionSave = _data.Clone();
        }


        public void LoadLastSessionSave()
        {
            _data = _sessionSave.Clone();

            _trash.Dispose();
            InitModels();
        }


        public bool IsChecked(string id)
        {
            return _checkpoints.Contains(id);
        }


        public void SetChecked(string id)
        {
            if (!_checkpoints.Contains(id))
            {
                SaveSession();
                _checkpoints.Add(id);
            }
        }


        private void OnDestroy()
        {
            _trash.Dispose();
        }


        private List<string> _removedItems = new List<string>();


        public bool RestoreState(string itemID)
        {
            return _removedItems.Contains(itemID);
        }


        public void StoreState(string itemID)
        {
            if (!_removedItems.Contains(itemID))
                _removedItems.Add(itemID);
        }


        public void StoreCheckpoints()
        {
            var currentSceneInfo = FindSceneManagementInfo(GetCurrentSceneName());
            currentSceneInfo.RenewStoredCheckpoints(_checkpoints);
        }


        public ScenesManagementInfo FindSceneManagementInfo(string value)
        {
            foreach (var scene in _scenesInfo)
            {
                if (scene.Id == value)
                {
                    return scene;
                }
            }
            return null;
        }


        public string GetCurrentSceneName()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            return currentSceneName;
        }


        public void ClearCheckpointList()
        {
            _checkpoints.Clear();
        }


        public void SetThatLevelWasFinished()
        {
            foreach (var scene in _scenesInfo)
            {
                if (scene.Id == GetCurrentSceneName())
                {
                    scene.ChangeSceneStatusFlag(true);
                    break;
                }
            }
        }


        public bool GetThatSceneWasFinished()
        {
            var checkpoints = FindObjectsOfType<CheckPointComponent>();
            foreach (var checkpoint in checkpoints)
            {
                if (checkpoint.Id == FindSceneManagementInfo(GetCurrentSceneName()).LevelExitCheckpoint)
                {
                    if (checkpoint.WasChecked == true)
                    {
                        return true;
                    }
                }
            }
            return false;
        }


        public void StoreSceneIndex()
        {
            var currentSceneInfo = FindSceneManagementInfo(GetCurrentSceneName());
            _storedSceneIndex = currentSceneInfo.SceneIndex;
        }


        public bool IsMoveToUpperIndexScene()
        {
            var currentSceneInfo = FindSceneManagementInfo(GetCurrentSceneName());
            return _storedSceneIndex < currentSceneInfo.SceneIndex ? true : false;
        }
    }
}

