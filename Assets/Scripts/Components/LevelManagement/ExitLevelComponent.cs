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
            session.ClearCheckpointList();
            session.StoreSceneIndex();
            session.SaveSession();
            loader.LoadLevel(_sceneName);
        }
    }
}
