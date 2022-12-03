﻿using Camera;
using UnityEngine;

namespace General.Components
{
    public class ShowTargetComponent : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private CameraStateController _controller;
        [SerializeField] private float _delay = 0.5f;


        private void OnValidate()
        {
            if (_controller == null)
            {
                _controller = FindObjectOfType<CameraStateController>();
            }
        }


        public void ShowTarget()
        {
            _controller.SetPosition(_target.position);
            _controller.SetState(true);
            Invoke(nameof(Moveback), _delay);
        }


        private void Moveback()
        {
            _controller.SetState(false);
        }
    }
}
