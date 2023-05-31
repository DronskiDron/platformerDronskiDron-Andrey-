﻿using System;
using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utils;

namespace UI.InGameMenu
{
    public class InGameMenuWindow : AnimatedWindow
    {
        private float _defaultTimeScale;
        private Action _closeAction;


        protected override void Start()
        {
            base.Start();
            _defaultTimeScale = Time.timeScale;
            Time.timeScale = 0;
        }


        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }


        public void OnLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
            Close();
        }


        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");

            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
        }


        private void OnDestroy()
        {
            Time.timeScale = _defaultTimeScale;
        }
    }
}