﻿using Player.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General.Components
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;
        public void Exit()
        {
            var session = FindObjectOfType<GameSession>();
            session.SaveSession();
            SceneManager.LoadScene(_sceneName);
        }
    }
}