using UnityEngine;

namespace BackGround
{
    public class BackGroundFollowScript : MonoBehaviour
    {
        [Header("BgFollow")]
        [SerializeField] private Transform _target;
        [SerializeField] private float _damping = 15f;
        [SerializeField] private bool _followMod = true;

        [Header("Paralax")]
        [SerializeField] private float _effectValue;


        private float _startX;


        private void Start()
        {
            SetObjectStartPosition();
        }


        private void LateUpdate()
        {
            if (_followMod)
                BackGroundFollowTarget();
            else
                ParalaxEffect();
        }


        public void SetObjectStartPosition()
        {
            transform.position = new Vector3(_target.position.x, transform.position.y, transform.position.z);
        }


        private void BackGroundFollowTarget()
        {
            var destination = new Vector3(_target.position.x, transform.position.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, destination, Time.deltaTime * _damping);
        }


        private void ParalaxEffect()
        {
            var currentPosition = transform.position;
            var deltaX = _target.position.x * _effectValue;
            transform.position = new Vector3(_startX + deltaX, currentPosition.y, currentPosition.z);
        }
    }
}

