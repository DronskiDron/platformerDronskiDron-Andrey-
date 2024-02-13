using Creatures.Player;
using UnityEngine;

namespace Creatures.Weapons
{
    public class Projectile : BaseProjectile
    {
        [SerializeField] private bool _belongToPlayer = false;

        private PlayerController _playerController;
        private bool _wasFirstShot = false;


        protected override void Start()
        {
            base.Start();
            Push();

            _wasFirstShot = true;
            if (_belongToPlayer)
            {
                _playerController = FindObjectOfType<PlayerController>();
            }
        }


        private void OnEnable()
        {
            if (_wasFirstShot)
            {
                if (_belongToPlayer)
                    Direction = _playerController.transform.lossyScale.x > 0 ? 1 : -1;
                Push();
            }
        }


        private void Push()
        {
            var force = new Vector2(Direction * Speed, 0);
            Rigidbody.AddForce(force, ForceMode2D.Impulse);
        }
    }
}
