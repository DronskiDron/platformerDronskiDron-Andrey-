using UnityEngine;

namespace Utils
{
    public static class WindowUtils
    {
        private static string _tag = "UI";


        public static void CreateWindow(string resourcePath)
        {
            var window = Resources.Load<GameObject>(resourcePath);
            var canvas = GameObject.FindGameObjectWithTag(_tag);
            Object.Instantiate(window, canvas.transform);
        }
    }
}
