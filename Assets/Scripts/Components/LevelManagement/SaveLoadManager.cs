using System.IO;
using Creatures.Model.Data;
using UnityEngine;

namespace General.Components.LevelManagement
{
    public class SaveLoadManager : MonoBehaviour
    {
        [SerializeField] private GameSession _session;


        private void Start()
        {
            _session = GameSession.Instance;
        }


        public void SaveData()
        {
            var filePath = Path.Combine(Application.persistentDataPath, "session.json");
            var json = JsonUtility.ToJson(_session);
            File.WriteAllText(filePath, json);
        }


        public void ResetData()
        {
            var filePath = Path.Combine(Application.persistentDataPath, "session.json");
            var json = JsonUtility.ToJson(null);
            File.WriteAllText(filePath, json);
        }


        public void LoadData()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "session.json");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);
                JsonUtility.FromJsonOverwrite(json, _session);
            }
        }


        public bool ExistsSaveCheck()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "session.json");

            if (File.Exists(filePath))
            {
                return true;
            }
            else
                return false;
        }


        public GameSession LoadSaveDataToMenu(GameSession session)
        {
            string filePath = Path.Combine(Application.persistentDataPath, "session.json");

            if (File.Exists(filePath))
            {
                var json = File.ReadAllText(filePath);

                JsonUtility.FromJsonOverwrite(json, session);

                return session;
            }
            else
            {
                return null;
            }
        }




    }
}
