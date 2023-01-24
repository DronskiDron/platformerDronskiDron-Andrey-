using UnityEngine;

namespace Creatures.Weapons
{
    public class SinusoidalProjectile : BaseProjectile
    {
        [SerializeField] private float _frequency = 1f;
        [SerializeField] private float _amplitude = 1f;

        private float _originalY;
        private float _time;
        protected override void Start()
        {
            base.Start();
            _originalY = Rigidbody.position.y;
        }


        private void FixedUpdate()
        {
            SetGameObjectTrajectory();
        }


        private void SetGameObjectTrajectory()
        {
            var position = Rigidbody.position;
            position.x += Direction * Speed;
            position.y = _originalY + Mathf.Cos(_time * _frequency) * _amplitude;
            Rigidbody.MovePosition(position);
            _time += Time.fixedDeltaTime;
        }
    }
}
