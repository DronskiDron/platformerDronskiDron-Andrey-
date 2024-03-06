using System;
using System.Collections;
using General.Components.ColliderBased;
using UnityEngine;
using UnityEngine.Events;

namespace Creatures.Patrolling
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerCheck _obstacleCheck;
        [SerializeField] private int _direction;
        [SerializeField] private OnChangeDirection _onChangeDirection;

        private MobAI _mobAI;

        private void Awake()
        {
            _mobAI = GetComponent<MobAI>();
        }


        public override IEnumerator DoPatrol()
        {
            while (enabled && !_mobAI.IsDead)
            {
                if (_groundCheck.IsTouchingLayer && !_obstacleCheck.IsTouchingLayer)
                {
                    _onChangeDirection?.Invoke(new Vector2(_direction, 0));
                }
                else if (!_mobAI.IsFollow)
                {
                    _direction = -_direction;
                    _onChangeDirection?.Invoke(new Vector2(_direction, 0));
                }

                yield return null;
            }
        }


        public override IEnumerator DoOneMorePatrol()
        {
            while (enabled)
            {
                if (_groundCheck.IsTouchingLayer && !_obstacleCheck.IsTouchingLayer)
                {
                    _onChangeDirection?.Invoke(new Vector2(_direction, 0));
                }
                else
                {
                    _direction = -_direction;
                    _onChangeDirection?.Invoke(new Vector2(_direction, 0));
                }

                yield return new WaitForSeconds(0.1f);
            }
        }
    }


    [Serializable]
    public class OnChangeDirection : UnityEvent<Vector2>
    {

    }


}
