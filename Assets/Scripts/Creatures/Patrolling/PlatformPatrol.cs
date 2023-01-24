using System.Collections;
using General.Components.ColliderBased;
using UnityEngine;

namespace Creatures.Patrolling
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private LayerCheck _obstacleCheck;
        [SerializeField] private int _direction;
        [SerializeField] private Creature _creature;

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
                    _creature.SetMoveDirection(new Vector2(_direction, 0));
                }
                else if (!_mobAI.IsFollow)
                {
                    _direction = -_direction;
                    _creature.SetMoveDirection(new Vector2(_direction, 0));
                }

                yield return null;
            }
        }
    }
}
