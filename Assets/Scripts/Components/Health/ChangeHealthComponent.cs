using General.Components.Creatures;
using UnityEngine;

namespace General.Components.Health
{
    public class ChangeHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _changeHealthValue;


        public void ChangeHealth(GameObject target)
        {
            var healthComponent = target.GetComponent<HealthComponent>();
            healthComponent?.RenewHealth(_changeHealthValue);
        }
    }
}

