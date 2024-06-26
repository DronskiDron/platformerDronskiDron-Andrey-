﻿using System;
using Creatures.Player;
using UnityEngine;

namespace General.Components.Cutscenes
{
    public class ShowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private ShowTargetController _controller;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private InputEnableComponent _inputEnabler;
        [SerializeField] private bool _manualCameraReturn = false;
        [SerializeField] private bool _activateInputAfterAnimation = true;

        public static Action OnCameraReturn;


        private void OnValidate()
        {
            if (_controller == null)
            {
                _controller = FindObjectOfType<ShowTargetController>();
            }
        }


        public void ShowTarget()
        {
            _inputEnabler.SetInputActivationStatus(false);
            _inputEnabler.SetInputDisabled();
            _controller.SetPosition(_target.position);
            _controller.SetState(true);

            if (!_manualCameraReturn)
                Invoke(nameof(Moveback), _delay);
        }


        private void Moveback()
        {
            _controller.SetState(false);
            OnCameraReturn?.Invoke();

            if (_activateInputAfterAnimation)
            {
                _inputEnabler.SetInputActivationStatus(true);
                _inputEnabler.SetInputEnabled();
            }
        }


        public void MoveCameraBack()
        {
            Moveback();
        }
    }
}
