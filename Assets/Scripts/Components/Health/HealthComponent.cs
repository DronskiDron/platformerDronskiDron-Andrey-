﻿using System;
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
        private bool _isShielUse = false;

        public int Health => _health;
        public bool IsShieldUse { get => _isShielUse; set => _isShielUse = value; }


        public bool IsPlayer = false;
        private GameSession _session;


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();

            if (IsPlayer)
            {
                PlayerHealtUpgrade();
                StatWidget.UpgradeStatHealth += PlayerHealtUpgrade;
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
            if (_health <= 0) return;

            if (_isImmortal)
            {
                gotHelth = 0;
            }

            _health += gotHelth;

            if (gotHelth > 0)
            {
                _onHeal?.Invoke();
                if (_health > _startHealth) _health = _startHealth;
            }
            else
            {
                _onDamage?.Invoke();
                if (_health <= 0)
                {
                    _onDie?.Invoke();
                }
            }
            if (_target.CompareTag("Player"))
            {
                Debug.Log($"У Вас осталось {_health} жизней");
            }
            _onChange?.Invoke(_health);

        }


        public void BecomeImmortal()
        {
            if (_target.CompareTag("Player"))
            {
                _isImmortal = true;
                Debug.Log($"Я бессмертен!");
            }
        }


        public void BecomeMortal()
        {
            if (_target.CompareTag("Player") && !IsShieldUse)
            {
                _isImmortal = false;
                Debug.Log($"Я смертен(");
            }
        }


        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
            StatWidget.UpgradeStatHealth -= PlayerHealtUpgrade;
        }


        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
    }
}