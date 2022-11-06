using System;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Cheats
{
    public class CheatController : MonoBehaviour
    {
        [SerializeField] private float _inputTimeToLive;
        [SerializeField] private CheatItem[] _cheats;
        private StringBuilder _currentInput;

        private float _inputTime;


        private void Awake()
        {
            Keyboard.current.onTextInput += OnTextInput;
            _currentInput = new StringBuilder();
        }


        private void Update()
        {
            if (_inputTime < 0)
            {
                _currentInput.Clear();
            }
            else
            {
                _inputTime -= Time.deltaTime;
            }
        }


        private void OnTextInput(char inputChar)
        {
            _currentInput.Append(inputChar);
            _inputTime = _inputTimeToLive;
            FindAnyCheats();
        }


        private void FindAnyCheats()
        {
            var currentInput = _currentInput.ToString();

            foreach (var cheatItem in _cheats)
            {
                if (currentInput.Contains(cheatItem.Name))
                {
                    cheatItem.Action?.Invoke();
                    _currentInput.Clear();
                }
            }
        }
    }

    [Serializable]
    public class CheatItem
    {
        public string Name;
        public UnityEvent Action;
    }
}
