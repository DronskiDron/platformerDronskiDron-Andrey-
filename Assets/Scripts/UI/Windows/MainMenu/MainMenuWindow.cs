using System;
using Creatures.Model.Data;
using General.Components.LevelManagement;
using UI.LevelsLoader;
using UnityEngine;
using Utils;

namespace UI.Windows.MainMenu
{
    public class MainMenuWindow : AnimatedWindow
    {
        [SerializeField] private MainMenuSession _session;

        private Action _closeAction;


        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
        }


        public void OnStartGame()
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
                var jsonLoader = FindObjectOfType<SaveLoadManager>();
                jsonLoader.ResetData();
                loader.LoadLevel("Tutorial");
            };
            Close();
        }


        public void OnContinueGame()
        {
            _closeAction = () =>
            {
                var loader = FindObjectOfType<LevelLoader>();
                var jsonLoader = FindObjectOfType<SaveLoadManager>();
                var lastScene = jsonLoader.LoadSaveDataToMenu(_session)?.CurrentScene;
                if (lastScene != "" && lastScene != null)
                    loader.LoadLevel(lastScene);
                else
                {
                    loader.LoadLevel("Tutorial");
                }
            };
            Close();
        }


        public void OnLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }


        public void OnExit()
        {
            _closeAction = () =>
            {
                Application.Quit();

#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
            };
            Close();
        }


        public override void OnCloseAnimationComplete()
        {
            _closeAction?.Invoke();
            base.OnCloseAnimationComplete();
        }
    }
}
