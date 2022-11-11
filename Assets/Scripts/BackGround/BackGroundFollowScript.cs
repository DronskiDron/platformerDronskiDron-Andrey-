using UnityEngine;

namespace BackGround
{
    public class BackGroundFollowScript : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _damping = 15f;


        private void Start()
        {
            SetObjectStartPosition();
        }


        private void LateUpdate()
        {
            BackGroundFollowTarget();
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
    }
}

