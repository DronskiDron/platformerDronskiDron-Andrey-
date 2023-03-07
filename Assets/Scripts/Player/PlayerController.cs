using UnityEngine;
using UnityEditor.Animations;
using Utils;
using Creatures.Model.Data;
using System.Collections;
using General.Components.ColliderBased;

namespace Creatures.Player
{
    public class PlayerController : Creature, ICanAddInInventory
    {
        [Header("Player Params")]
        [SerializeField] private float _slamDownVelocity = 15f;
        [SerializeField] private float _slamDownDamageVelocity = 22f;
        [SerializeField] private int _slamDownDamageValue = 1;

        [Header("Player Checkers")]
        [SerializeField] private CheckCircleOverlap _interactionCheck;
        [SerializeField] private ColliderCheck _wallCheck;
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

        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count("Sword");


        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
        }


        protected override void Start()
        {
            base.Start();
            _session = FindObjectOfType<GameSession>();
            _session.Data.Hp.Value = Health.Health;
            _session.Data.Inventory.OnChanged += OnInventoryChanged;
            _session.Data.Inventory.OnChanged += InventoryLogger;

            _startSlamDownDamageVelocity = _slamDownDamageVelocity;
            UpdatePlayerWeapon();
        }


        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnInventoryChanged;
            _session.Data.Inventory.OnChanged -= InventoryLogger;
        }


        private void InventoryLogger(string id, int value)
        {
            Debug.Log($"Congratulations! You have got: {id}:{value}");
        }


        private void OnInventoryChanged(string id, int value)
        {
            if (id == "Sword")
                UpdatePlayerWeapon();
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
                _allowDoubleJump = false;
                _wasDoubleJump = true;
                DoJumpVfx();
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


        public void AddInInventory(string id, int value)
        {
            _session.Data.Inventory.Add(id, value);
        }


        public override void TakeDamage()
        {
            base.TakeDamage();
            if (CoinCount > 0)
            {
                SpawnCoins();
            }
        }


        private void SpawnCoins()
        {
            var numCoinsToDespose = Mathf.Min(CoinCount, 5);
            _session.Data.Inventory.Remove("Coin", numCoinsToDespose);

            var burst = _hitParticles.emission.GetBurst(0);
            burst.count = numCoinsToDespose;
            _hitParticles.emission.SetBurst(0, burst);
            _hitParticles.gameObject.SetActive(true);
            _hitParticles.Play();
        }


        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp.Value = currentHealth;
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
            if (SwordCount <= 0) return;

            base.Attack();
        }


        private void UpdatePlayerWeapon()
        {
            Animator.runtimeAnimatorController = SwordCount > 0 ? _armed : _disarmed;
        }


        public void Throw()
        {
            if (_throwCooldown.IsReady && SwordCount > 1)
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
            Sounds.Play("Range");
            Particles.Spawn("Throw");
            _session.Data.Inventory.Remove("Sword", 1);
        }


        public void ThrowAction()
        {
            if (_superThrow)
            {
                var numThrows = Mathf.Min(_superThrowParticles, SwordCount - 1);
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


        public void UsePotion()
        {
            var potionCount = _session.Data.Inventory.Count("HealthPotion");
            if (potionCount > 0)
            {
                Health.RenewHealth(5);
                _session.Data.Inventory.Remove("HealthPotion", 1);
            }
        }
    }
}

