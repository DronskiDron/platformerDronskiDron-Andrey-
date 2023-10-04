using Creatures.Model.Data;
using Creatures.Model.Data.Models;
using Creatures.Model.Definitions.Player;
using UnityEngine;

namespace General.Components.Health
{
    public class ChangeHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _changeHealthValue;
        [SerializeField] private bool _isBelongToPlayer = false;
        [SerializeField] private bool _isProjectile = false;

        private GameSession _session;
        private int _referenceChangeHealthValue;


        private void Start()
        {
            if (_isBelongToPlayer)
            {
                _session = FindObjectOfType<GameSession>();
                _referenceChangeHealthValue = _changeHealthValue;

                if (_isProjectile)
                {
                    UpgradeRangeDamage();
                    ModifyDamageByCrit();
                }
            }
        }


        private void UpgradeRangeDamage()
        {
            var statsModel = new StatsModel(_session.Data);
            var currenStattValue = (int)statsModel.GetValue(StatId.RangeDamage);
            _changeHealthValue = -currenStattValue;
        }


        public void ChangeHealth(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            healthComponent?.RenewHealth(_changeHealthValue);
        }


        public void ModifyDamageByCrit()
        {
            var statsModel = new StatsModel(_session.Data);
            var critChance = (int)statsModel.GetValue(StatId.CriticalDamage);
            _changeHealthValue = _referenceChangeHealthValue;

            if (Random.value * 100 <= critChance)
            {
                _changeHealthValue *= 2;
            }
        }
    }
}

