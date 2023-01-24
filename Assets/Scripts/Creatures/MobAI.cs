using System.Collections;
using Creatures.Patrolling;
using General.Components.ColliderBased;
using General.Components.Creatures;
using UnityEngine;

namespace Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] protected ColliderCheck Vision;
        [SerializeField] protected ColliderCheck CanAttack;
        [SerializeField] private float _agrDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 0.2f;
        [SerializeField] private float _beforeAttackDelay = 0;
        [SerializeField] protected float MissPlayerCooldown = 1f;

        private Coroutine _current;
        protected GameObject Target;
        protected SpawnListComponent Particles;
        protected Creature Creature;
        private Animator _animator;
        private bool _isDead;
        public bool IsDead => _isDead;

        protected Patrol Patrol;
        private bool _isFollow = false;
        public bool IsFollow { get => _isFollow; set => _isFollow = value; }

        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");


        private void Awake()
        {
            Particles = GetComponent<SpawnListComponent>();
            Creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            Patrol = GetComponent<Patrol>();
        }


        private void Start()
        {
            StartState(Patrol.DoPatrol());
        }


        public virtual void OnPlayerInVision(GameObject go)
        {
            if (_isDead) return;

            Target = go;
            StartState(AgroToPlayer());
        }


        protected virtual IEnumerator AgroToPlayer()
        {
            LookAtPlayer();
            Particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_agrDelay);
            StartState(GoToPlayer());
        }


        protected virtual void LookAtPlayer()
        {
            var direction = GetDirectionToTarget();
            Creature.SetMoveDirection(Vector2.zero);
            Creature.UpdateSpriteDirection(direction);
        }


        protected virtual IEnumerator GoToPlayer()
        {
            while (Vision.IsTouchingLayer)
            {
                if (CanAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }

                yield return null;
            }

            Creature.SetMoveDirection(Vector2.zero);
            Particles.Spawn("MissPlayer");
            yield return new WaitForSeconds(MissPlayerCooldown);
            StartState(Patrol.DoPatrol());
        }


        protected virtual IEnumerator Attack()
        {
            while (CanAttack.IsTouchingLayer)
            {
                yield return new WaitForSeconds(_beforeAttackDelay);
                Creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            StartState(GoToPlayer());
        }


        protected void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            Creature.SetMoveDirection(direction);
        }


        protected virtual Vector2 GetDirectionToTarget()
        {
            var direction = Target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }


        protected void StartState(IEnumerator coroutine)
        {
            Creature.SetMoveDirection(Vector2.zero);
            if (_current != null) StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }


        public void OnDie()
        {
            Creature.SetMoveDirection(Vector2.zero);
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);

            if (_current != null) StopCoroutine(_current);
        }
    }
}
