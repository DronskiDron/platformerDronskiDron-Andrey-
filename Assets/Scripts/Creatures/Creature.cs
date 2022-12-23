﻿using General.Components.ColliderBased;
using General.Components.Creatures;
using General.Components.Health;
using UnityEngine;

namespace Creatures
{
    public class Creature : MonoBehaviour
    {
        [Header("Creature Params")]
        [SerializeField] private float _speed = 5f;
        [SerializeField] protected float JumpForce = 1f;
        [SerializeField] private float _damageJumpForce = 10f;
        [SerializeField] protected HealthComponent Health;
        [SerializeField] private bool _invertScale;
        public bool InvertScale => _invertScale;

        [Header("Creature Checkers")]
        [SerializeField] private CheckCircleOverlap _attackRange;
        [SerializeField] protected SpawnListComponent Particles;
        [SerializeField] protected LayerCheckCreatures GroundCheck;

        protected Rigidbody2D Rigidbody;
        protected Vector2 MoveDirection;
        protected Animator Animator;
        protected bool IsGroundedNow;
        protected bool IsJumping;

        private static readonly int IsRunning = Animator.StringToHash("is-Running");
        private static readonly int IsGround = Animator.StringToHash("is-Ground");
        private static readonly int VerticalVelocity = Animator.StringToHash("vertical-Velocity");
        private static readonly int Hit = Animator.StringToHash("hit");
        private static readonly int Heal = Animator.StringToHash("heal");
        private static readonly int AttackKey = Animator.StringToHash("attack");

        protected virtual void Awake()
        {
            Rigidbody = GetComponent<Rigidbody2D>();
            Animator = GetComponent<Animator>();
        }


        protected virtual void Start()
        {
            Health = GetComponent<HealthComponent>();
        }


        protected virtual void FixedUpdate()
        {
            PlayerMover();
            UpdateSpriteDirection(MoveDirection);
            AniimationSwitcher();
        }


        protected virtual void Update()
        {
            IsGroundedNow = IsGrounded();
        }


        public void SetMoveDirection(Vector2 direction)
        {
            MoveDirection = direction;
        }


        private bool IsGrounded()
        {
            var isGrounded = GroundCheck.GetIsGrounded();
            return isGrounded;
        }


        private void PlayerMover()
        {
            var xVelocity = MoveDirection.x * _speed;
            var yVelocity = CalculateVelocity();
            Rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        }


        protected virtual float CalculateVelocity()
        {
            var yVelocity = Rigidbody.velocity.y;
            var isJumpPressing = GroundCheck.GetIsPressingJump();
            if (IsGroundedNow)
            {
                IsJumping = false;
            }

            if (isJumpPressing)
            {
                IsJumping = true;

                var isFalling = Rigidbody.velocity.y <= 0.001f;
                yVelocity = isFalling ? CalculateJumpVelocity(yVelocity) : yVelocity;
            }
            else if (Rigidbody.velocity.y > 0 && IsJumping)
            {
                yVelocity *= 0.5f;
            }

            return yVelocity;
        }


        protected virtual float CalculateJumpVelocity(float yVelocity)
        {
            if (IsGroundedNow)
            {
                yVelocity += JumpForce;
                Particles.Spawn("Jump");
            }

            return yVelocity;
        }


        private void AniimationSwitcher()
        {
            Animator.SetBool(IsRunning, MoveDirection.x != 0);
            Animator.SetBool(IsGround, IsGroundedNow);
            Animator.SetFloat(VerticalVelocity, Rigidbody.velocity.y);
        }


        public void UpdateSpriteDirection(Vector2 direction)
        {
            var multiplier = _invertScale ? -1 : 1;
            if (MoveDirection.x > 0)
            {
                transform.localScale = new Vector3(multiplier, 1, 1);
            }
            else if (MoveDirection.x < 0)
            {
                transform.localScale = new Vector3(-1 * multiplier, 1, 1);
            }
        }


        public virtual void TakeDamage()
        {
            IsJumping = false;
            Animator.SetTrigger(Hit);
            Rigidbody.velocity = new Vector2(Rigidbody.velocity.x, _damageJumpForce);
        }


        public void Healing()
        {
            Animator.SetTrigger(Heal);
        }


        public virtual void Attack()
        {
            Animator.SetTrigger(AttackKey);
        }


        protected virtual void AttackAction()
        {
            Particles.Spawn("SwordAttack");
            _attackRange.Check();
        }
    }
}
