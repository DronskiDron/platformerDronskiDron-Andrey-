using System.Reflection;
using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General.Components.LevelManagement
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = GameSession.Instance;
            session.Loader.LoadData();
            session.StoreSceneIndex();

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
#if UNITY_EDITOR
            ClearLog();
#endif
        }


        public void ReloadFromBegining()
        {
            var session = GameSession.Instance;
            session.TheGameWasRestarted = true;
            session.ItemStateStorage.ClearRemoveItemsList(session.GetCurrentSceneName());
            session.ClearLocalCheckpointList();
            session.ClearStoredCurrentLevelCheckpoints();
            session.GetCurrentSceneManagementInfo().ChangeSceneStatusFlag(false);
            session.LoadLastLocalSessionSave();
            session.StoreSceneIndex();
            session.Loader.SaveData();

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
#if UNITY_EDITOR
            ClearLog();
#endif
        }


#if UNITY_EDITOR
        public void ClearLog()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
#endif
    }
}

