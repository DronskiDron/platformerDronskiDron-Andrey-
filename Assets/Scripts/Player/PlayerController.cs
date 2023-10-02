using UnityEngine;
using UnityEditor.Animations;
using Utils;
using Creatures.Model.Data;
using System.Collections;
using General.Components.ColliderBased;
using General.Components;
using Creatures.Model.Definitions;
using Creatures.Model.Definitions.Items;
using General.Components.Health;
using Creatures.Model.Definitions.Player;

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
        [SerializeField] private SpawnComponent _throwSpawner;
        [SerializeField] private ShieldComponent _shield;

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
        private HealthComponent _healthComponent;

        private const string SwordId = "Sword";
        private int CoinCount => _session.Data.Inventory.Count("Coin");
        private int SwordCount => _session.Data.Inventory.Count(SwordId);
        private string SelectedItemId => _session.QuickInventory.SelectedItem.Id;

        private bool CanThrow
        {
            get
            {
                if (SelectedItemId == SwordId)
                    return SwordCount > 1;

                var def = DefsFacade.I.Items.Get(SelectedItemId);
                return def.HasTag(ItemTag.Throwable);
            }
        }

        private readonly Cooldown _speedUpCooldown = new Cooldown();
        private float _additionalSpeed;


        protected override void Awake()
        {
            base.Awake();
            _defaultGravityScale = Rigidbody.gravityScale;
        }


        protected override void Start()
        {
            base.Start();
            _session = FindObjectOfType<GameSession>();
            _healthComponent = FindObjectOfType<HealthComponent>();
            _session.Data.Hp.Value = (int)_session.StatsModel.GetValue(StatId.Hp); ;
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
            if (id == SwordId)
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
            if (!IsGroundedNow && _allowDoubleJump && _session.PerksModel.IsDoubleJumpSupported && !_isOnWall)
            {
                _session.PerksModel.Cooldown.Reset();
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
            if (_healthComponent.IsShieldUse)
                return;
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


        public void UseInventory()
        {
            if (IsSelectedItem(ItemTag.Throwable))
                Throw();

            else if (IsSelectedItem(ItemTag.Potion))
                UsePotion();

        }


        private void UsePotion()
        {
            var potion = DefsFacade.I.Potions.Get(SelectedItemId);

            switch (potion.Effect)
            {
                case Model.Definitions.Repository.Effect.AddHp:
                    Health.RenewHealth((int)potion.Value);
                    break;
                case Model.Definitions.Repository.Effect.SpeedUp:
                    _speedUpCooldown.Value = _speedUpCooldown.RemainingTime + potion.Time;
                    _additionalSpeed = Mathf.Max(potion.Value, _additionalSpeed);
                    _speedUpCooldown.Reset();
                    break;
            }

            _session.Data.Inventory.Remove(potion.Id, 1);
        }


        protected override float CalculateSpeed()
        {
            if (_speedUpCooldown.IsReady)
                _additionalSpeed = 0f;

            var defaultSpeed = _session.StatsModel.GetValue(StatId.Speed);
            return defaultSpeed + _additionalSpeed;
        }


        private bool IsSelectedItem(ItemTag tag)
        {
            return _session.QuickInventory.SelectedDef.HasTag(tag);
        }


        internal void Throw()
        {
            if (!_throwCooldown.IsReady || !CanThrow) return;

            Animator.SetTrigger(ThrowKey);
            _throwCooldown.Reset();
        }


        public void IsSuperThrowAvailable(bool value)
        {
            _superThrow = value;
        }


        private void ThrowAndRemoveFromInventory()
        {
            Sounds.Play("Range");

            var throwableId = _session.QuickInventory.SelectedItem.Id;
            var throwableDef = DefsFacade.I.Throwable.Get(throwableId);
            _throwSpawner.SetPrefab(throwableDef.Projectile);
            _throwSpawner.Spawn();
            _session.Data.Inventory.Remove(throwableId, 1);
        }


        public void ThrowAction()
        {
            if (_superThrow && _session.PerksModel.IsSuperThrowSupported)
            {
                var throwableCount = _session.Data.Inventory.Count(SelectedItemId);
                var possibleCount = SelectedItemId == SwordId ? throwableCount - 1 : throwableCount;

                var numThrows = Mathf.Min(_superThrowParticles, possibleCount);
                _session.PerksModel.Cooldown.Reset();
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


        internal void NextItem()
        {
            _session.QuickInventory.SetNextItem();
        }


        public void UsePerk()
        {
            if (_session.PerksModel.IsShieldSupported)
            {
                _shield.Use();
                _session.PerksModel.Cooldown.Reset();
            }
        }
    }
}

