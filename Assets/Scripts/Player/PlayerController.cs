using UnityEngine;
using General.Components;
using System.Diagnostics;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerJumpChecker _playerJumpChecker;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _jumpForce = 1;
        [SerializeField] private float _damageJumpForce = 10;
        [SerializeField] private float _interactionRadius = 5;
        [SerializeField] private LayerMask _interactionLayer;
        [SerializeField] private SpawnComponent _footStepParticles;
        [SerializeField] private SpawnComponent _jumpDustParticles;
        [SerializeField] private SpawnComponent _landingDustParticles;
        [SerializeField] private ParticleSystem _hitParticles;
        [SerializeField] private CoinCounter _coinCounter;

        const float BASE_FALLING_TIME = 500f;
        private Collider2D[] _interactionResult = new Collider2D[1];
        private Rigidbody2D _rigidbody;
        private Vector2 _moveDirection;
        private Animator _animator;
        private bool _isGrounded;
        private bool _allowDoubleJump;
        private bool _isJumping;
        private bool _isHardLanding = false;
        private Stopwatch _watch = new Stopwatch();
        private bool _isFallTimerStarted = false;
        private bool _doubleJumpWasUsed = false;


        private static readonly int IsRunning = Animator.StringToHash("is-Running");
        private static readonly int IsGround = Animator.StringToHash("is-Ground");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-Velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int Heal = Animator.StringToHash("heal");


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
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
            LongFalling();
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
            }
            else if (_allowDoubleJump)
            {
                SpawnJumpDust();
                _isHardLanding = true;
                _doubleJumpWasUsed = true;
                yVelocity = _jumpForce;
                _allowDoubleJump = false;
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

            if (_coinCounter.Money > 0)
            {
                SpawnCoins();
            }
        }


        private void SpawnCoins()
        {
            var numCoinsToDespose = Mathf.Min(_coinCounter.Money, 5);
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


        public void SpawnJumpDust()
        {
            if (_allowDoubleJump || _isGrounded)
            {
                _jumpDustParticles.Spawn();
            }
        }


        public void SpawnLandingDust()
        {
            if (_isHardLanding && _isGrounded)
            {
                _landingDustParticles.Spawn();
            }
            _isHardLanding = false;
        }


        private void LongFalling()
        {
            var yVelocity = _rigidbody.velocity.y;

            if (yVelocity < 0 && !_isFallTimerStarted)
            {
                _watch.Start();
                _isFallTimerStarted = true;
            }
            else if (yVelocity >= 0 && _isFallTimerStarted)
            {
                _watch.Stop();
                FallingTimeChecker();
                _isFallTimerStarted = false;
            }
        }


        public void FallingTimeChecker()
        {
            var timeSpan = _watch.ElapsedMilliseconds;

            if (timeSpan >= BASE_FALLING_TIME && !_doubleJumpWasUsed)
            {
                _isHardLanding = true;
            }

            _watch.Reset();
            _doubleJumpWasUsed = false;
        }
    }
}

