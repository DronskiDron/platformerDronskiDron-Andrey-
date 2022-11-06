using UnityEngine;
using UnityEngine.Events;

namespace General.Components.Creatures
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;

        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onDie;

        private int _startHealth;
        private bool _isImmortal = false;


        private void Start()
        {
            _startHealth = _health;
        }


        public void RenewHealth(int gotHelth)
        {
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
            Debug.Log($"У Вас осталось {_health} жизней");
        }


        public void BecomeImmortal()
        {
            _isImmortal = true;
            Debug.Log($"Я бессмертен!");
        }


        public void BecomeMortal()
        {
            _isImmortal = false;
            Debug.Log($"Я смертен(");
        }
    }
}