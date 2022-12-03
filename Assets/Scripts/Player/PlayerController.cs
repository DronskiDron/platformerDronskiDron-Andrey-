using UnityEngine;
using General.Components;
using General.Components.Creatures;
using UnityEditor.Animations;
using Utils;
using Player.Model;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerJumpChecker _playerJumpChecker;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpForce = 1f;
        [SerializeField] private float _damageJumpForce = 10f;
        [SerializeField] private float _slamDownVelocity = 15f;
        [SerializeField] private float _slamDownDamageVelocity = 22f;
        [SerializeField] private int _slamDownDamage = 1;
        [SerializeField] private float _interactionRadius = 5f;
        [SerializeField] private int _damage = 1;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private CoinCounter _coinCounter;
        [SerializeField] private HealthComponent _health;

        [SerializeField] private AnimatorController _armed;
        [SerializeField] private AnimatorController _disarmed;

        [SerializeField] private CheckCircleOverlap _attackRange;

        [Space]
        [Header("Particles")]
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpDustParticles;
        [SerializeField] private SpawnComponent _slamDownDustParticles;
        [SerializeField] private SpawnComponent _swordAttackParticles;

        [SerializeField] private ParticleSystem _hitParticles;

        private readonly Collider2D[] _interactionResult = new Collider2D[1];
        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _wasDoubleJump = false;
        private bool _isJumping;

        private bool _isAllowSlamDownParticle = true;
        private float _startSlamDownDamageVelocity;
        public float StartSlamDownDamageVelocity => _startSlamDownDamageVelocity;

        private static readonly int IsRunning = Animator.StringToHash("is-Running");
        private static readonly int IsGround = Animator.StringToHash("is-Ground");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-Velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int Heal = Animator.StringToHash("heal");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        private GameSession _session;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
        }


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
            _health = GetComponent<HealthComponent>();
            _session.Data.Hp = _health.Health;
            UpdatePlayerWeapon();
            _startSlamDownDamageVelocity = _slamDownDamageVelocity;
        }


        private void FixedUpdate()
        {
            PlayerMover();
            AniimationSwitcher();
        }


        private void Update()
        {
            _isGrounded = IsGrounded();
        }


        public void SetMoveDirection(Vector2 direction)
        {
            _moveDirection = direction;
        }


        private bool IsGrounded()
        {
            var isGrounded = _playerJumpChecker.GetIsGrounded();
            return isGrounded;
        }


        public void SaySomething()
        {
            UnityEngine.Debug.Log("ПАЛУНДРА!!!");
        }


        private void PlayerMover()
        {
            var xVelocity = _moveDirection.x * _speed;
            var yVelocity = CalculateVelocity();
            _rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        }


        private float CalculateVelocity()
        {
            var yVelocity = _rigidbody.velocity.y;
            var isJumpPressing = _playerJumpChecker.GetIsPressingJump();

            if (_isGrounded)
            {
                _allowDoubleJump = true;
                _isJumping = false;
            }

            if (isJumpPressing)
            {
                _isJumping = true;
                yVelocity = CalculateJumpVelocity(yVelocity);
            }
            else if (_rigidbody.velocity.y > 0 && _isJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }


        private float CalculateJumpVelocity(float yVelocity)
        {
            var isFalling = _rigidbody.velocity.y <= 0.001f;

            if (!isFalling) return yVelocity;

            if (_isGrounded)
            {
                yVelocity += _jumpForce;
                _jumpDustParticles.Spawn();
            }
            else if (_allowDoubleJump)
            {
                _jumpDustParticles.Spawn();
                yVelocity = _jumpForce;
                _allowDoubleJump = false;
                _wasDoubleJump = true;
            }

            return yVelocity;
        }


        private void AniimationSwitcher()
        {
            _animator.SetBool(IsRunning, _moveDirection.x != 0);
            _animator.SetBool(IsGround, _isGrounded);
            _animator.SetFloat(VerticalVelocity, _rigidbody.velocity.y);

            if (_moveDirection.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            else if (_moveDirection.x < 0)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }


        public void TakeDamage()
        {
            _isJumping = false;
            _animator.SetTrigger(Hit);
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _damageJumpForce);

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


        public void Healing()
        {
            _animator.SetTrigger(Heal);
        }


        public void OnHealthChanged(int currentHealth)
        {
            _session.Data.Hp = currentHealth;
        }


        public void Interact()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _interactionRadius,
                _interactionResult,
                _interactionLayer);

            for (int i = 0; i < size; i++)
            {
                var interactable = _interactionResult[i].GetComponent<InteractableComponent>();
                interactable?.Interact();
            }
        }


        public void SpawnFootDust()
        {
            _footStepParticles.Spawn();
        }


        public void SpawnSwordAttackParticles()
        {
            _swordAttackParticles.Spawn();
        }


        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.IsInLayer(_playerJumpChecker.GroundLayer))
            {
                var contact = collision.contacts[0];
                if (contact.relativeVelocity.y >= _slamDownVelocity && _isAllowSlamDownParticle || _wasDoubleJump)
                {
                    _slamDownDustParticles.Spawn();
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
            _health.RenewHealth(-_slamDownDamage);
        }


        public void SetCurrentSlamDownDamageVelocity(float value)
        {
            _slamDownDamageVelocity = value;
        }


        public void AllowSlamDownParticle(bool value)
        {
            _isAllowSlamDownParticle = value;
        }


        internal void Attack()
        {
            if (!_session.Data.IsArmed) return;

            _animator.SetTrigger(AttackKey);
        }


        public void AttackAction()
        {
            SpawnSwordAttackParticles();
            var gos = _attackRange.GetObjectsInRange();
            foreach (var go in gos)
            {
                var hp = go.GetComponent<HealthComponent>();
                if (hp != null && hp.gameObject.CompareTag("Enemy"))
                {
                    hp.RenewHealth(-_damage);
                }
            }
        }


        internal void ArmPlayer()
        {
            _session.Data.IsArmed = true;
            UpdatePlayerWeapon();
        }


        private void UpdatePlayerWeapon()
        {
            _animator.runtimeAnimatorController = _session.Data.IsArmed ? _armed : _disarmed;
        }
    }
}

