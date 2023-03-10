using System.Collections.Generic;
using UnityEngine;

namespace General.Components.Health
{
    public class CompositeHealthComponent : HealthComponent
    {
        private List<HealthComponent> _compositeHp = new List<HealthComponent>();


        private void Awake()
        {
            ListFiller();
            TotalHpCounter();
        }


        private void ListFiller()
        {
            var commonHp = GetComponentsInChildren<HealthComponent>();
            foreach (var value in commonHp)
            {
                _compositeHp.Add(value);
            }
        }


        private void TotalHpCounter()
        {
            foreach (var value in _compositeHp)
            {
                _health += value.Health;
            }
        }
    }
}
