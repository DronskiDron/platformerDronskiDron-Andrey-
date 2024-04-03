using System;
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
        [HideInInspector] public bool TheGameWasRestarted = false;

        public PlayerData Data => _data;
        public string CurrentScene => _currentScene;
        public SaveLoadManager Loader => _loader;
        public static GameSession Instance { get; set; }

        private readonly CompositeDisposable _trash = new CompositeDisposable();


        protected virtual void Awake()
        {
            var existsSession = GetExistsSession();
            var currentSceneInfo = GetCurrentSceneManagementInfo();
            ItemStateStorage.InitDestroyedItemsContainers(_scenesInfo);
            if (existsSession != null)
            {
                existsSession.StartSession(currentSceneInfo.GetActualLevelCheckpoint());
                Destroy(gameObject);
            }
            else
            {
                if (_loader.ExistsSaveCheck())
                    _loader.LoadData();
                InitModels();
                DontDestroyOnLoad(this);
                Instance = this;
                StartSession(currentSceneInfo.GetActualLevelCheckpoint());
            }
        }


        protected virtual void Start()
        {
            if (_loader == null)
                _loader = GetComponent<SaveLoadManager>();

            _loader.SaveData();
        }


        private void StartSession(string defaultCheckPoint)
        {
            LoadHud();
            SpawnPlayer();
        }


        private void SpawnPlayer()
        {
            var currentSceneInfo = GetCurrentSceneManagementInfo();
            var checkpoints = FindObjectsOfType<CheckPointComponent>();

            if (currentSceneInfo.GetSceneStatusFlag() && GetLevelsMoveProgress() == LevelProgressStatus.Up || TheGameWasRestarted)
            {
                var enterCheckpoint = currentSceneInfo.LevelEnterCheckpoint;
                TrySpawnPlayerOnGivenCheckpoint(enterCheckpoint, checkpoints);
            }
            else if (currentSceneInfo.GetSceneStatusFlag() && GetLevelsMoveProgress() == LevelProgressStatus.Down)
            {
                var exitCheckpoint = currentSceneInfo.LevelExitCheckpoint;
                TrySpawnPlayerOnGivenCheckpoint(exitCheckpoint, checkpoints);
            }
            else
            {
                var actualCheckpoint = currentSceneInfo.GetActualLevelCheckpoint();
                TrySpawnPlayerOnGivenCheckpoint(actualCheckpoint, checkpoints);
            }
        }


        private void TrySpawnPlayerOnGivenCheckpoint(string checkpointId, CheckPointComponent[] checkpoints)
        {
            foreach (var checkPoint in checkpoints)
            {
                if (checkPoint.Id == checkpointId)
                {
                    checkPoint.SpawnPlayer();
                    TheGameWasRestarted = false;
                    LocalSaveSession();
                    _loader?.SaveData();
                    break;
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


        public void LocalSaveSession()
        {
            _currentScene = GetCurrentSceneName();
            _sessionSave = _data.Clone();
        }


        public void LoadLastLocalSessionSave()
        {
            _data = _sessionSave.Clone();

            _trash.Dispose();
            InitModels();
        }


        protected virtual void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
            _trash.Dispose();
        }


        public void StoreAnyCheckpoint(string checkpointName)
        {
            var currentSceneInfo = GetCurrentSceneManagementInfo();
            currentSceneInfo.StoreCheckpoint(checkpointName);
        }


        public void ClearStoredCurrentLevelCheckpoints()
        {
            var currentSceneInfo = GetCurrentSceneManagementInfo();
            currentSceneInfo.ClearCheckpointList();
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


        public ScenesManagementInfo GetCurrentSceneManagementInfo()
        {
            var currentSceneName = GetCurrentSceneName();
            var currentSceneInfo = FindSceneManagementInfo(currentSceneName);
            return currentSceneInfo;
        }


        public string GetCurrentSceneName()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            return currentSceneName;
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
                if (checkpoint.Id == GetCurrentSceneManagementInfo().LevelExitCheckpoint)
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
            var currentSceneInfo = GetCurrentSceneManagementInfo();
            _storedSceneIndex = currentSceneInfo.SceneIndex;
        }


        private LevelProgressStatus GetLevelsMoveProgress()
        {
            var currentSceneInfo = GetCurrentSceneManagementInfo();

            if (_storedSceneIndex < currentSceneInfo.SceneIndex)
                return LevelProgressStatus.Up;
            else if (_storedSceneIndex > currentSceneInfo.SceneIndex)
                return LevelProgressStatus.Down;
            else return LevelProgressStatus.WithoutChanges;

        }
    }


    public enum LevelProgressStatus
    {
        Up,
        Down,
        WithoutChanges
    }
}

