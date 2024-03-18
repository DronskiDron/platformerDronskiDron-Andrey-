using System;
using Creatures.Model.Data;
using Creatures.Model.Data.Models;
using Creatures.Model.Definitions.Player;
using UI.Windows.Perks.PlayerStats;
using UnityEngine;
using UnityEngine.Events;

namespace General.Components.Health
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] protected int _health;
        [SerializeField] private GameObject _target;

        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] public HealthChangeEvent _onChange;

        private int _startHealth;
        private bool _isImmortal = false;
        private bool _isShieldUse = false;
        public bool IsPlayer = false;
        private GameSession _session;

        public int Health => _health;
        public bool IsShieldUse { get => _isShieldUse; set => _isShieldUse = value; }
        public UnityEvent OnDamage => _onDamage;


        private void Start()
        {
            _session = GameSession.Instance;

            if (IsPlayer)
            {
                PlayerHealtUpgrade();
                PlayerStatsWindow.UpgradeStatsAction += PlayerHealtUpgrade;
            }
            else
            {
                _startHealth = _health;
            }
        }


        private void PlayerHealtUpgrade()
        {
            var statsModel = new StatsModel(_session.Data);
            _startHealth = (int)statsModel.GetValue(StatId.Hp);
            _health = _startHealth;
        }


        public void RenewHealth(int gotHelth)
        {
            if (gotHelth == 0 || _health <= 0) return;

            if (_isImmortal && gotHelth < 0)
                return;

            _health += gotHelth;

            if (gotHelth > 0)
            {
                _onHeal?.Invoke();
                if (_health > _startHealth) _health = _startHealth;
            }
            else if (gotHelth < 0 && _isImmortal == false)
            {
                _onDamage?.Invoke();

                if (_health <= 0)
                {
                    _onDie?.Invoke();
                }
            }

            _onChange?.Invoke(_health);
        }


        public void BecomeImmortal()
        {
            if (_target.CompareTag("Player"))
                _isImmortal = true;
        }


        public void BecomeMortal()
        {
            if (_target.CompareTag("Player") && !IsShieldUse)
                _isImmortal = false;
        }


        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
            PlayerStatsWindow.UpgradeStatsAction -= PlayerHealtUpgrade;
        }


        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
    }
}