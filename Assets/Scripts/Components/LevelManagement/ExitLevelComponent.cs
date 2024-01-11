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
            var session = FindObjectOfType<GameSession>();
            var loader = FindObjectOfType<LevelLoader>();

            if (session.GetThatSceneWasFinished())
                session.SetThatLevelWasFinished();
            session.StoreCheckpoints();
            session.StoreSceneIndex();
            session.SaveSession();
            session.ClearCheckpointList();
            loader.LoadLevel(_sceneName);
        }
    }
}
