using System.Collections.Generic;
using System.Linq;
using General.Components;
using General.Components.Health;
using UnityEngine;
using Utils;

namespace Creatures
{
    public class TotemTower : MonoBehaviour
    {
        [SerializeField] private List<ShootingTrapAI> _traps;
        [SerializeField] private Cooldown _cooldown;

        private int _currentTrap;
        private DestroyObjectComponent _destructor;


        private void Start()
        {
            _destructor = GetComponent<DestroyObjectComponent>();

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
                _destructor.DestroyObject();
            }
            var hasAnyTarget = _traps.Any(x => x.Vision.IsTouchingLayer);

            if (hasAnyTarget)
            {
                if (_cooldown.IsReady)
                {
                    _traps[_currentTrap].Shoot();
                    _cooldown.Reset();
                    _currentTrap = (int)Mathf.Repeat(_currentTrap + 1, _traps.Count);
                }
            }
        }
    }
}
