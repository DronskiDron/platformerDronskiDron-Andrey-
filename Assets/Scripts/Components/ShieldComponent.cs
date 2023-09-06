using General.Components.Health;
using UnityEngine;
using Utils;

namespace Creatures
{
    public class ShieldComponent : MonoBehaviour
    {
        [SerializeField] private HealthComponent _health;
        [SerializeField] private Cooldown _cooldown;

        public void Use()
        {
            _health.IsShieldUse = true;
            _health.BecomeImmortal();
            _cooldown.Reset();
            gameObject.SetActive(true);
        }


        private void Update()
        {
            if (_cooldown.IsReady)
                gameObject.SetActive(false);
        }


        private void OnDisable()
        {
            _health.IsShieldUse = false;
            _health.BecomeMortal();
        }

    }
}
