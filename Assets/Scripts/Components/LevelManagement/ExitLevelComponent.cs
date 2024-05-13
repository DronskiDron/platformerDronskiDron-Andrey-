using Creatures.Model.Data;
using PirateIsland.Analytics;
using UI.LevelsLoader;
using UnityEngine;

namespace General.Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;


        public void Exit()
        {
            var session = GameSession.Instance;
            var loader = FindObjectOfType<LevelLoader>();
            // PirateIslandAnalytics.TrackThatLevelWasCompleted(session.GetCurrentSceneManagementInfo().SceneIndex);

            if (session.GetThatSceneWasFinished())
                session.SetThatLevelWasFinished();
            session.StoreSceneIndex();
            session.LocalSaveSession();
            session.Loader.SaveData();
            loader.LoadLevel(_sceneName);
        }
    }
}
