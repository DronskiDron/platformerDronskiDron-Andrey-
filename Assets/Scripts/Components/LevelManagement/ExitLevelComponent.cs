using Creatures.Model.Data;
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

            if (session.GetThatSceneWasFinished())
                session.SetThatLevelWasFinished();
            session.ClearLocalCheckpointList();
            session.StoreSceneIndex();
            session.LocalSaveSession();
            session.Loader.SaveData();
            loader.LoadLevel(_sceneName);
        }
    }
}
