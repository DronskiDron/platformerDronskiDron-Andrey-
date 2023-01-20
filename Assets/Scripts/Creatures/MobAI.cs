using System.Collections;
using Creatures.Patrolling;
using General.Components.ColliderBased;
using General.Components.Creatures;
using UnityEngine;

namespace Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private ColliderCheck _vision;
        [SerializeField] private ColliderCheck _canAttack;
        [SerializeField] private float _agrDelay = 0.5f;
        [SerializeField] private float _attackCooldown = 0.2f;
        [SerializeField] private float _missPlayerCooldown = 1f;

        private Coroutine _current;
        private GameObject _target;
        private SpawnListComponent _particles;
        private Creature _creature;
        private Animator _animator;
        private bool _isDead;
        public bool IsDead => _isDead;
        private Patrol _patrol;

        private static readonly int IsDeadKey = Animator.StringToHash("is-dead");


        private void Awake()
        {
            _particles = GetComponent<SpawnListComponent>();
            _creature = GetComponent<Creature>();
            _animator = GetComponent<Animator>();
            _patrol = GetComponent<Patrol>();
        }


        private void Start()
        {
            StartState(_patrol.DoPatrol());
        }


        public void OnPlayerInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;
            StartState(AgroToPlayer());
        }


        private IEnumerator AgroToPlayer()
        {
            LookAtPlayer();
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_agrDelay);
            StartState(GoToPlayer());
        }

        private void LookAtPlayer()
        {
            var direction = GetDirectionToTarget();
            _creature.SetMoveDirection(Vector2.zero);
            _creature.UpdateSpriteDirection(direction);
        }

        private IEnumerator GoToPlayer()
        {
            while (_vision.IsTouchingLayer)
            {
                if (_canAttack.IsTouchingLayer)
                {
                    StartState(Attack());
                }
                else
                {
                    SetDirectionToTarget();
                }

                yield return null;
            }

            _creature.SetMoveDirection(Vector2.zero);
            _particles.Spawn("MissPlayer");
            yield return new WaitForSeconds(_missPlayerCooldown);
            StartState(_patrol.DoPatrol());
        }


        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            StartState(GoToPlayer());
        }


        private void SetDirectionToTarget()
        {
            var direction = GetDirectionToTarget();
            _creature.SetMoveDirection(direction);
        }


        private Vector2 GetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            return direction.normalized;
        }


        private void StartState(IEnumerator coroutine)
        {
            _creature.SetMoveDirection(Vector2.zero);
            if (_current != null) StopCoroutine(_current);

            _current = StartCoroutine(coroutine);
        }


        public void OnDie()
        {
            _creature.SetMoveDirection(Vector2.zero);
            _isDead = true;
            _animator.SetBool(IsDeadKey, true);

            if (_current != null) StopCoroutine(_current);
        }
    }
}
