using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General.Components.LevelManagement
{
    public class ExitLevelComponent : MonoBehaviour
    {
        [SerializeField] private string _sceneName;


        public void Exit()
        {
            var session = FindObjectOfType<GameSession>();

            if (session.GetThatSceneWasFinished())
                session.SetThatLevelWasFinished();
            session.StoreCheckpoints();
            session.StoreSceneIndex();
            session.SaveSession();
            session.ClearCheckpointList();
            SceneManager.LoadScene(_sceneName);
        }
    }
}
