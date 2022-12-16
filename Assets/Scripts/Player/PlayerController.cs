using UnityEngine;
using UnityEditor.Animations;
using Utils;
using Creatures.Model;
using System;

namespace Creatures.Player
{
    public class PlayerController : Creature
    {
        [Header("Player Params")]
        [SerializeField] private float _slamDownVelocity = 15f;
        [SerializeField] private float _slamDownDamageVelocity = 22f;
        [SerializeField] private int _slamDownDamageValue = 1;

        [Header("Player Checkers")]
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private CoinCounter _coinCounter;

        [Header("Weapon States")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;

        private bool _allowDoubleJump;
        private bool _wasDoubleJump = false;
        private bool _isAllowSlamDownParticle = true;
        private float _startSlamDownDamageVelocity;
        public float StartSlamDownDamageVelocity => _startSlamDownDamageVelocity;

        private GameSession _session;


        protected override void Start()
        {
            base.Start();
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp = Health.Health;
            UpdatePlayerWeapon();
            _startSlamDownDamageVelocity = _slamDownDamageVelocity;
        }


        protected override float CalculateVelocity()
        {
            if (IsGroundedNow)
            {
                _allowDoubleJump = true;
            }

            return base.CalculateVelocity();
        }


        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGroundedNow && _allowDoubleJump)
            {
                Particles.Spawn("Jump");
                _allowDoubleJump = false;
                _wasDoubleJump = true;
                return JumpForce;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }


        public override void TakeDamage()
        {
            base.TakeDamage();
            if (_session.Data.Coins > 0)
            {
                SpawnCoins();
            }
        }


        private void SpawnCoins()
        {
            var numCoinsToDespose = Mathf.Min(_session.Data.Coins, 5);
            _coinCounter.GetMoney(-numCoinsToDespose);

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDespose;
            _hitParticles.emission.SetBurst(0, burst);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }


        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }


        public void Interact()
        {
            _interactionCheck.Check();
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(GroundCheck.GroundLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity && _isAllowSlamDownParticle || _wasDoubleJump)
                {
                    Particles.Spawn("SlamDown");
                    if (contact.relativeVelocity.y >= _slamDownDamageVelocity)
                    {
                        SlamDownDamage();
                    }
                }
                _wasDoubleJump = false;
            }
        }


        private void SlamDownDamage()
        {
            Health.RenewHealth(-_slamDownDamageValue);
        }


        public void SetCurrentSlamDownDamageVelocity(float value)
        {
            _slamDownDamageVelocity = value;
        }


        public void AllowSlamDownParticle(bool value)
        {
            _isAllowSlamDownParticle = value;
        }


        public override void Attack()
        {
            if (!_session.Data.IsArmed) return;

            base.Attack();
        }


        internal void ArmPlayer()
        {
            _session.Data.IsArmed = true;
            UpdatePlayerWeapon();
        }


        private void UpdatePlayerWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
        }


        internal void Throw()
        {
            throw new NotImplementedException();
        }
    }
}

