using General.Components;
using Player;
using UnityEngine;

namespace Props
{
    public class BarrelScript : MonoBehaviour
    {
        [SerializeField] private SpawnComponent _landingDustParticles;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SimplexSpriteAnimation _simplexAnimator;

        const float MAX_BARREL_Y_VELOCITY = 3.508889E-10f;
        private bool _isGrounded = false;


        public void PlayFallingDustAnimation()
        {
            if (_isGrounded)
            {
                _landingDustParticles.Spawn();
            }
        }


        public void GroundCheck()
        {
            _isGrounded = true;
        }


        public void ImmortalWhileFalling(GameObject target)
        {
            var player = target.GetComponent<PlayerController>();
            var yVelocity = _rigidbody.velocity.y;

            if (yVelocity > MAX_BARREL_Y_VELOCITY)
            {
                player?.SetCurrentFallingTime(0);
            }
        }


        public void StopSpawnLandingDust(GameObject target)
        {
            var player = target.GetComponent<PlayerController>();
            var yVelocity = _rigidbody.velocity.y;

            if (yVelocity > MAX_BARREL_Y_VELOCITY)
            {
                player?.SpawnLandingDustResolver(false);
            }
        }


        public void PlayBarrelAnimation(GameObject target)
        {
            _simplexAnimator.StartAnimationNow();
        }
    }
}
