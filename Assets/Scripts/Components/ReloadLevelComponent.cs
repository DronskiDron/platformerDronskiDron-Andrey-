using System.Reflection;
using Creatures.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace General.Components
{
    public class ReloadLevelComponent : MonoBehaviour
    {
        public void Reload()
        {
            var session = FindObjectOfType<GameSession>();
            session.LoadLastSessionSave();

            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
            ClearLog();
        }


        public void ClearLog()
        {
            var assembly = Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}

