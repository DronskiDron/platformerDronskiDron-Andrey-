using System;
using System.Collections;
using UnityEngine;

namespace General.Components.Health
{
    [RequireComponent(typeof(HealthComponent))]
    public class TakeDamageDelayComponent : MonoBehaviour
    {
        [SerializeField] private float _delayAfterLastDamage = 5f;

        private HealthComponent _health;
        private Coroutine _coroutine;


        private void Start()
        {
            _health = GetComponent<HealthComponent>();
            _health.OnDamage.AddListener(StartDelayCoroutine);
        }


        private void StartDelayCoroutine()
        {
            if (_coroutine == null)
                _coroutine = StartCoroutine(RunDelay());
        }


        private IEnumerator RunDelay()
        {
            _health.BecomeImmortal();
            yield return new WaitForSeconds(_delayAfterLastDamage);
            _health.BecomeMortal();
            _coroutine = null;
        }


        private void OnDestroy()
        {
            _health.OnDamage.RemoveListener(StartDelayCoroutine);
        }
    }
}
