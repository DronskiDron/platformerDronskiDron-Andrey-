﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace General.Components.Cutscenes
{
    public class ShowTargetComponentBoss : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private UnityEvent _onDelay;

        [SerializeField] private ShowTargetController _controller;

        private Coroutine _coroutine;


        private void OnValidate()
        {
            if (_controller == null)
            {
                _controller = FindObjectOfType<ShowTargetController>();
            }
        }


        public void Play()
        {
            _controller.SetPosition(_target.position);
            _controller.SetState(true);

            if (_coroutine != null)
                StopCoroutine(_coroutine);
            _coroutine = StartCoroutine(WaitAndReturn());
        }


        private IEnumerator WaitAndReturn()
        {
            yield return new WaitForSeconds(_delay);

            _onDelay?.Invoke();
            _controller.SetState(false);
        }
    }
}
