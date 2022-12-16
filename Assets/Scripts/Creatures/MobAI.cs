using System.Collections;
using General.Components;
using General.Components.Creatures;
using UnityEngine;

namespace Creatures
{
    public class MobAI : MonoBehaviour
    {
        [SerializeField] private LayerCheck _vision;
        [SerializeField] private LayerCheck _canAttack;
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
        private bool _isAttackingNow = false;

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


        public void OnHeroeInVision(GameObject go)
        {
            if (_isDead) return;

            _target = go;
            StartState(AgroToHero());
        }


        private IEnumerator AgroToHero()
        {
            _particles.Spawn("Exclamation");
            yield return new WaitForSeconds(_agrDelay);
            StartState(GoToHero());
        }


        private IEnumerator GoToHero()
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

            _particles.Spawn("MissPlayer");
            yield return new WaitForSeconds(_missPlayerCooldown);
            StartState(_patrol.DoPatrol());
        }


        private IEnumerator Attack()
        {
            while (_canAttack.IsTouchingLayer)
            {
                _isAttackingNow = true;
                _creature.Attack();
                yield return new WaitForSeconds(_attackCooldown);
            }
            _isAttackingNow = false;
            StartState(GoToHero());
        }


        private void SetDirectionToTarget()
        {
            var direction = _target.transform.position - transform.position;
            direction.y = 0;
            _creature.SetMoveDirection(direction.normalized);
        }


        public void OnPatrol()
        {
            if (!_isAttackingNow)
            {
                StartState(_patrol.DoPatrol());
            }
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
