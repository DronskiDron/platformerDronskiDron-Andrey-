using System.Collections.Generic;
using General.Components.Health;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace Creatures
{
    public class TotemTower : MonoBehaviour
    {
        [SerializeField] private List<ShootingTrapAI> _traps;
        [SerializeField] private Cooldown _cooldown;
        [SerializeField] private UnityEvent _onDestroy;

        private int _currentTrap;


        private void Start()
        {
            foreach (var shootingTrapAI in _traps)
            {
                shootingTrapAI.enabled = false;
                var hp = shootingTrapAI.GetComponent<HealthComponent>();
                hp._onDie.AddListener(() => OnTrapDead(shootingTrapAI));
            }
        }


        private void OnTrapDead(ShootingTrapAI shootingTrapAI)
        {
            var index = _traps.IndexOf(shootingTrapAI);
            _traps.Remove(shootingTrapAI);
            if (index < _currentTrap)
            {
                _currentTrap--;
            }
        }


        private void Update()
        {
            if (_traps.Count == 0)
            {
                enabled = false;
                _onDestroy?.Invoke();
            }

            if (HasAnyTarget())
            {
                if (_cooldown.IsReady)
                {
                    _traps[_currentTrap].Shoot();
                    _cooldown.Reset();
                    _currentTrap = (int)Mathf.Repeat(_currentTrap + 1, _traps.Count);
                }
            }
        }


        private bool HasAnyTarget()
        {
            foreach (var shootingTrapAI in _traps)
            {
                if (shootingTrapAI.Vision.IsTouchingLayer)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
