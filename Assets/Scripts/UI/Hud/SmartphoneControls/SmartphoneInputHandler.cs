using UI.Windows;
using UnityEngine;

namespace UI.Hud.SmartphoneControls
{
    public class SmartphoneInputHandler : MonoBehaviour
    {
        [SerializeField] private GameObject[] _controls;

        public static SmartphoneInputHandler Instance { get; private set; }


        private void Start()
        {
            Instance = this;
            AnimatedWindow.OnWindowAppeared += DisableControls;
        }


        public void EnableControls()
        {
            // Debug.Log("AAAAA");
            foreach (var control in _controls)
            {
                control?.SetActive(true);
            }
        }


        public void DisableControls()
        {
            // Debug.Log("BBBBB");
            foreach (var control in _controls)
            {
                control?.SetActive(false);
            }
        }


        private void OnDestroy()
        {
            AnimatedWindow.OnWindowAppeared -= DisableControls;
        }


    }
}
