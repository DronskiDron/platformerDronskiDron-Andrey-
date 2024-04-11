using UI.Hud.SmartphoneControls;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Creatures.Player
{
    public class InputEnableComponent : MonoBehaviour
    {
        public static bool IsInputActive = true;

        private PlayerInput _input;


        private void Start()
        {
            _input = FindObjectOfType<PlayerInput>();
        }


        public void SetInputEnabled()
        {
            if (IsInputActive == true)
                _input.enabled = true;
            // Debug.Log("AAAAA");
            // #if USE_ONSCREEN_CONTROLS
            SmartphoneInputHandler.Instance.EnableControls();
            // #endif
        }


        public void SetInputDisabled()
        {
            if (IsInputActive == false)
                _input.enabled = false;
            // Debug.Log("BBBBB");
            // #if USE_ONSCREEN_CONTROLS
            SmartphoneInputHandler.Instance.DisableControls();
            // #endif
        }


        public void SetInputActivationStatus(bool value)
        {
            IsInputActive = value;
        }
    }
}
