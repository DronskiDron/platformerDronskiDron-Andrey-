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
        }


        public void SetInputDisabled()
        {
            if (IsInputActive == false)
                _input.enabled = false;
        }


        public void SetInputActivationStatus(bool value)
        {
            IsInputActive = value;
        }
    }
}
