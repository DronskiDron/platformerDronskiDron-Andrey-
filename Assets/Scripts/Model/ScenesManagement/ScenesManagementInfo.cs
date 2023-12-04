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

        private List<string> _storedCheckpoints = new List<string>();
        private bool _levelWasFinished = false;

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


        public void RenewStoredCheckpoints(List<string> list)
        {
            _storedCheckpoints = new List<string>(list);
        }
    }
}

