using System;
using Creatures.Model.Data;
using General.Components.TimeManipulation;
using UnityEngine.SceneManagement;
using Utils;

namespace UI.Windows.InGameMenu
{
    public class InGameMenuWindow : AnimatedWindow
    {
        private Action _closeAction;


        protected override void Start()
        {
            base.Start();
            TimeManipulator.StopTime();
        }


        public void OnShowSettings()
        {
            WindowUtils.CreateWindow("UI/SettingsWindow");
            Close();
        }


        public void OnLanguages()
        {
            WindowUtils.CreateWindow("UI/LocalizationWindow");
        }


        public void OnExit()
        {
            SceneManager.LoadScene("MainMenu");

            var session = FindObjectOfType<GameSession>();
            Destroy(session.gameObject);
        }


        private void OnDestroy()
        {
            TimeManipulator.RunTimeNormal();
        }
    }
}
