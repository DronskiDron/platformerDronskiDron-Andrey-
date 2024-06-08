using UI.Hud.SmartphoneControls;
using UnityEngine;

namespace Creatures.Player
{
    public class InputEnableComponent : MonoBehaviour
    {
        [SerializeField] private float _delayValue = 0;
        [SerializeField] private float[] _delayArray;

        private static bool IsInputActive = true;
        public static bool IsMenusActive { get; private set; }


        public void SetInputEnabled()
        {
            Invoke("SetEnabled", _delayValue);
        }


        public void SetInputEnabledWithCustomDelay(int index)
        {
            Invoke("SetEnabled", _delayArray[index]);
        }


        public void SetInputDisabled()
        {
            Invoke("SetDisabled", _delayValue);
        }


        public void SetInputDisabledWithCustomDelay(int index)
        {
            Invoke("SetDisabled", _delayArray[index]);
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
            IsInputActive = false;
        }


        private void SetEnabled()
        {
            if (IsInputActive == true)
                PlayerInputReader.IsInputEnable = true;
#if USE_ONSCREEN_CONTROLS
            if (Application.isMobilePlatform)
                SmartphoneInputHandler.Instance.EnableControls();
#endif
        }


        private void SetDisabled()
        {
            if (IsInputActive == false)
                PlayerInputReader.IsInputEnable = false;
#if USE_ONSCREEN_CONTROLS
            if (Application.isMobilePlatform)
                SmartphoneInputHandler.Instance.DisableControls();
#endif
        }


        public static void ToggleMenusActivationStatus(bool value)
        {
            IsMenusActive = value;
        }
    }
}
