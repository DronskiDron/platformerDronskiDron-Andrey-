using UnityEngine;

namespace Player.Model
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private PlayerData _data;
        public PlayerData Data => _data;
        private PlayerData _sessionSave;


        private void Awake()
        {
            if (IsSessionExit())
            {
                Destroy(gameObject);
            }
            else
            {
                DontDestroyOnLoad(this);
            }
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

