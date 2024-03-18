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
    [Serializable]
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        [SerializeField] private ScenesManagementInfo[] _scenesInfo;
        [SerializeField] private SaveLoadManager _loader;

        public QuickInventoryModel QuickInventory { get; private set; }
        public BigInventoryModel BigInventory { get; private set; }
        public PerksModel PerksModel { get; private set; }
        public StatsModel StatsModel { get; private set; }

        [HideInInspector] public readonly ItemsStateStorage ItemStateStorage = new ItemsStateStorage();

        [HideInInspector][SerializeField] private PlayerData _sessionSave;
        [HideInInspector][SerializeField] private int _storedSceneIndex;
        [HideInInspector][SerializeField] private string _currentScene;
        [HideInInspector][SerializeField] private List<string> _checkpoints = new List<string>();

        public PlayerData Data => _data;
        public string CurrentScene => _currentScene;
        public SaveLoadManager Loader => _loader;
        public static GameSession Instance { get; set; }

        private readonly CompositeDisposable _trash = new CompositeDisposable();


        protected virtual void Awake()
        {
            var existsSession = GetExistsSession();
            var currentSceneInfo = FindSceneManagementInfo(GetCurrentSceneName());
            ItemStateStorage.InitDestroyedItemsContainers(_scenesInfo);
            if (existsSession != null)
            {
                existsSession.StartSession(currentSceneInfo.LevelEnterCheckpoint);
                Destroy(gameObject);
            }
            else
            {
                if (_loader.ExistsSaveCheck())
                    _loader.LoadData();
                InitModels();
                DontDestroyOnLoad(this);
                Instance = this;
                StartSession(currentSceneInfo.LevelEnterCheckpoint);
            }
        }


        protected virtual void Start()
        {
            if (_loader == null)
                _loader = GetComponent<SaveLoadManager>();
            // SaveSession();
            _loader.SaveData();
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
            _currentScene = GetCurrentSceneName();
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


        protected virtual void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
            _trash.Dispose();
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

