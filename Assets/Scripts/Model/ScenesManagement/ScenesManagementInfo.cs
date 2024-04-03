using System;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures.Model.Data.ScenesManagement
{
    [Serializable]
    public class ScenesManagementInfo
    {
        [SerializeField] private string _id;
        [SerializeField] private int _sceneIndex;
        [SerializeField] private string _levelEnterCheckpoint;
        [SerializeField] private string _levelExitCheckpoint;
        [SerializeField] private string _actualLevelCheckpoint;
        [SerializeField] private List<string> _storedCheckpoints = new List<string>();
        [SerializeField] private bool _levelWasFinished = false;

        [HideInInspector][SerializeField] private List<string> _storedItems = new List<string>();

        public string Id => _id;
        public string LevelEnterCheckpoint => _levelEnterCheckpoint;
        public string LevelExitCheckpoint => _levelExitCheckpoint;
        public int SceneIndex => _sceneIndex;


        public void ChangeSceneStatusFlag(bool value)
        {
            _levelWasFinished = value;
        }


        public bool GetSceneStatusFlag()
        {
            return _levelWasFinished;
        }


        public void ClearCheckpointList()
        {
            _storedCheckpoints.Clear();
        }


        public void StoreCheckpoint(string checkpointName)
        {
            if (!_storedCheckpoints.Contains(checkpointName))
                _storedCheckpoints.Add(checkpointName);
        }


        public void SetActualLevelCheckpoint(string id)
        {
            _actualLevelCheckpoint = id;
        }


        public string GetActualLevelCheckpoint()
        {
            if (_actualLevelCheckpoint == "")
                return _levelEnterCheckpoint;
            else
                return _actualLevelCheckpoint;
        }


        public List<string> GetLevelStoredItems()
        {
            return _storedItems;
        }
    }
}

