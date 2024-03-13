using UnityEngine;

namespace BackGround
{
    public class MoveBackground : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 1f;
        [SerializeField] private float _offset;
        [SerializeField] private bool _rightMoveDirection = false;

        private Vector2 _startPosition;
        private float _newXposition;


        private void Start()
        {
            _startPosition = transform.position;
        }


        private void Update()
        {
            _newXposition = Mathf.Repeat(Time.time * (_rightMoveDirection ? 1 : -1) * _moveSpeed, _offset);
            transform.position = _startPosition + Vector2.right * _newXposition;
        }
    }
}
