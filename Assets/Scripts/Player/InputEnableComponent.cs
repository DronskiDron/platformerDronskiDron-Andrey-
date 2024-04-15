using UI.Hud.SmartphoneControls;
using UnityEngine;

namespace Creatures.Player
{
    public class InputEnableComponent : MonoBehaviour
    {
        [SerializeField] private float _delayValue = 0;
        private static bool IsInputActive = true;


        public void SetInputEnabled()
        {
            Invoke("SetEnabled", _delayValue);
        }


        public void SetInputDisabled()
        {
            Invoke("SetDisabled", _delayValue);
        }


        public void SetInputActivationStatus(bool value)
        {
            if (value)
                Invoke("SetStatusTrue", _delayValue);
            else
                Invoke("SetStatusFalse", _delayValue);
        }


        private void SetStatusTrue()
        {
            IsInputActive = true;
        }


        private void SetStatusFalse()
        {
            IsInputActive = true;
        }


        private void SetEnabled()
        {
            if (IsInputActive == true)
                PlayerInputReader.IsInputEnable = true;
#if USE_ONSCREEN_CONTROLS
            SmartphoneInputHandler.Instance.EnableControls();
#endif
        }


        private void SetDisabled()
        {
            if (IsInputActive == false)
                PlayerInputReader.IsInputEnable = false;
#if USE_ONSCREEN_CONTROLS
            SmartphoneInputHandler.Instance.DisableControls();
#endif
        }
    }
}
