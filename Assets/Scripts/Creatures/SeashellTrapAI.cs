using System;
using General.Components;
using General.Components.Audio;
using General.Components.ColliderBased;
using UnityEngine;
using Utils;

namespace Creatures
{
    public class SeashellTrapAI : MonoBehaviour
    {
        [Header("Melee")]
        [SerializeField] private ColliderCheck _vision;
        [SerializeField] private Cooldown _meleeCooldown;
        [SerializeField] private CheckCircleOverlap _meleeAttack;
        [SerializeField] private ColliderCheck _meleeCanAttack;

        [Header("Range")]
        [SerializeField] private Cooldown _rangeCooldown;
        [SerializeField] private SpawnComponent _rangeAttack;

        private static readonly int Melee = Animator.StringToHash("melee");
        private static readonly int Range = Animator.StringToHash("range");

        private Animator _animator;
        protected PlaySoundsComponent Sounds;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            Sounds = GetComponent<PlaySoundsComponent>();
        }


        private void Update()
        {
            ShootingTrapAttack();
        }


        private void ShootingTrapAttack()
        {
            if (_vision.IsTouchingLayer)
            {
                if (_meleeCanAttack.IsTouchingLayer)
                {
                    if (_meleeCooldown.IsReady)
                    {
                        MeleeAttack();
                        return;
                    }
                }

                if (_rangeCooldown.IsReady)
                {
                    RangeAttack();
                }
            }
        }

        private void RangeAttack()
        {
            _rangeCooldown.Reset();
            _animator.SetTrigger(Range);
            Sounds.Play("Range");
        }


        private void MeleeAttack()
        {
            _meleeCooldown.Reset();
            _animator.SetTrigger(Melee);
        }


        public void OnMeleeAttack()
        {
            _meleeAttack.Check();
            Sounds.Play("Melee");
        }


        public void OnRangeAttack()
        {
            _rangeAttack.Spawn();
        }
    }
}
