using UnityEngine;
using UnityEditor.Animations;
using Utils;
using Creatures.Model.Data;
using System.Collections;
using General.Components.Collectables;
using General.Components.ColliderBased;

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
        [SerializeField] private LayerCheck _wallCheck;
        [SerializeField] private CoinCounter _coinCounter;
        [SerializeField] private Cooldown _throwCooldown;

        [Header("Weapon States")]
        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        [Header("Super throw")]
        [SerializeField] private Cooldown _superThrowCooldown;
        [SerializeField] private int _superThrowParticles;
        [SerializeField] private float _superThrowDelay;

        [Header("Particles")]
        [SerializeField] private ParticleSystem _hitParticles;
        private static readonly int ThrowKey = Animator.StringToHash("throw");
        private static readonly int IsOnWall = Animator.StringToHash("is-on-wall");

        private bool _allowDoubleJump;
        private bool _isOnWall;
        private bool _wasDoubleJump = false;
        private bool _isAllowSlamDownParticle = true;
        private float _startSlamDownDamageVelocity;
        public float StartSlamDownDamageVelocity => _startSlamDownDamageVelocity;
        private bool _superThrow = false;

        private GameSession _session;
        private float _defaultGravityScale;


        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
        }


        protected override void Start()
        {
            base.Start();
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp = Health.Health;
            UpdatePlayerWeapon();
            _startSlamDownDamageVelocity = _slamDownDamageVelocity;
        }


        protected override void Update()
        {
            base.Update();
            WallClimb();
        }


        protected override float CalculateVelocity()
        {
            var isJumpPressing = MoveDirection.y > 0;

            if (IsGroundedNow || _isOnWall)
            {
                _allowDoubleJump = true;
            }

            if (!isJumpPressing && _isOnWall)
            {
                return 0f;
            }

            return base.CalculateVelocity();
        }


        protected override float CalculateJumpVelocity(float yVelocity)
        {
            if (!IsGroundedNow && _allowDoubleJump && !_isOnWall)
            {
                Particles.Spawn("Jump");
                _allowDoubleJump = false;
                _wasDoubleJump = true;
                return JumpForce;
            }

            return base.CalculateJumpVelocity(yVelocity);
        }


        private void WallClimb()
        {
            var moveToSameDirection = MoveDirection.x * transform.lossyScale.x > 0;
            if (_wallCheck.IsTouchingLayer && moveToSameDirection)
            {
                _isOnWall = true;
                Rigidbody.gravityScale = 0;
            }
            else
            {
                _isOnWall = false;
                Rigidbody.gravityScale = _defaultGravityScale;
            }
            Animator.SetBool(IsOnWall, _isOnWall);
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


        public void CollectSwords()
        {
            _session.Data.Swords += 1;
        }


        private void UpdatePlayerWeapon()
        {
            Animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
        }


        public void Throw()
        {
            if (_session.Data.IsArmed && _throwCooldown.IsReady && _session.Data.Swords > 1)
            {
                Animator.SetTrigger(ThrowKey);
                _throwCooldown.Reset();
            }
        }


        public void IsSuperThrowAvailable(bool value)
        {
            _superThrow = value;
        }


        private void ThrowAndRemoveFromInventory()
        {
            Particles.Spawn("Throw");
            _session.Data.Swords -= 1;
        }


        public void ThrowAction()
        {
            if (_superThrow)
            {
                var numThrows = Mathf.Min(_superThrowParticles, _session.Data.Swords - 1);
                StartCoroutine(DoSuperThrow(numThrows));
            }
            else
            {
                ThrowAndRemoveFromInventory();
            }
            _superThrow = false;
        }


        private IEnumerator DoSuperThrow(int numThrows)
        {
            for (int i = 0; i < numThrows; i++)
            {
                ThrowAndRemoveFromInventory();
                yield return new WaitForSeconds(_superThrowDelay);
            }
        }
    }
}

