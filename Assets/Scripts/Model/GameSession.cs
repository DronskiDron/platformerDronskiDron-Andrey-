using UnityEngine;
using UnityEngine.SceneManagement;

namespace Creatures.Model.Data
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        private PlayerData _sessionSave;


        private void Awake()
        {
            LoadHud();

            if (IsSessionExit())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
        }


        private void LoadHud()
        {
            SceneManager.LoadScene("Hud", LoadSceneMode.Additive);
        }


        private void Start()
        {
            SaveSession();
        }


        private bool IsSessionExit()
        {
            var sessions = FindObjectsOfType<GameSession>();
            foreach (var gameSession in sessions)
            {
                if (gameSession != this) return true;
            }
            return false;
        }


        public void SaveSession()
        {
            _sessionSave = _data.Clone();
        }


        public void LoadLastSessionSave()
        {
            _data = _sessionSave.Clone();
        }
    }
}

