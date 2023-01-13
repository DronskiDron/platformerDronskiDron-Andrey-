using System;
using General.Components.Animation;
using General.Components.ColliderBased;
using UnityEngine;
using Utils;

namespace Creatures
{
    public class ShootingTrapAI : MonoBehaviour
    {
        [SerializeField] public ColliderCheck Vision;
        [SerializeField] private Cooldown _cooldown;
        [SerializeField] private SpriteAnimation _animation;


        private void Update()
        {
            if (Vision.IsTouchingLayer && _cooldown.IsReady)
            {
                Shoot();
            }
        }


        public void Shoot()
        {
            _cooldown.Reset();
            _animation.SetClip("start-attack");
        }
    }
}
