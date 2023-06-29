using System.Collections;
using Creatures.Player;
using General.Components.ColliderBased;
using UnityEngine;

namespace Creatures
{
    public class CrabbyAI : MobAI
    {
        [SerializeField] private LayerCheck _mobJumpChecker;


        protected override IEnumerator GoToPlayer()
        {
            IsFollow = true;
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
            IsFollow = false;
        }


        protected override Vector2 GetDirectionToTarget()
        {
            if (Target == null)
            {
                Target = FindObjectOfType<PlayerController>().gameObject;
            }
            var direction = Target.transform.position - transform.position;
            return direction.normalized;
        }


        protected override IEnumerator AgroToPlayer()
        {
            yield return base.AgroToPlayer();
        }


        private void Update()
        {
            if (_mobJumpChecker.IsTouchingLayer && IsFollow && !IsDead)
            {
                Creature.StartMobJump();
            }
        }
    }
}
