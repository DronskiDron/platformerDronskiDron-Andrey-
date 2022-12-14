﻿using System.Collections;
using General.Components;
using UnityEngine;

namespace Creatures
{
    public class PlatformPatrol : Patrol
    {
        [SerializeField] private LayerCheck _groundCheck;
        [SerializeField] private int _direction;
        [SerializeField] private Creature _creature;


        public override IEnumerator DoPatrol()
        {
            while (enabled)
            {
                if (_groundCheck.IsTouchingLayer)
                {
                    _creature.SetMoveDirection(new Vector2(_direction, 0));
                }
                else
                {
                    _direction = -_direction;
                    _creature.SetMoveDirection(new Vector2(_direction, 0));
                    yield return null;
                    yield return null;
                }
                yield return null;
            }
        }
    }
}
