using Creatures.Player;
using UnityEngine;

namespace BackGround
{
    public class BackgroundMovement : MonoBehaviour
    {
        // _playerTransform = FindObjectOfType<PlayerController>().transform;
        [SerializeField] private GameObject _background;
        [SerializeField] private Transform _leftBorder;
        [SerializeField] private Transform _rightBorder;
        [SerializeField] private Transform _leftSceneBorder;
        [SerializeField] private Transform _rightSceneBorder;
        [SerializeField] private Transform _playerTransform;

        private float _bgrSpriteWidth;
        private float _distance;
        private float _distancePercent;


        void Start()
        {
            // _playerTransform = FindObjectOfType<PlayerController>().transform;

            _bgrSpriteWidth = _rightBorder.position.x - _leftBorder.position.x;
            _distance = Vector2.Distance(_leftSceneBorder.position, _rightSceneBorder.position);
            _distancePercent = _distance / 100;
        }


        void Update()
        {
            MoveBackground();
        }


        private void MoveBackground()
        {
            var currentFinishPath = Vector2.Distance(_leftSceneBorder.position, _playerTransform.position);
            var finishedPathInPercent = currentFinishPath / _distancePercent;
            var offset = finishedPathInPercent / 100 * (_bgrSpriteWidth);
            _background.transform.localPosition = new Vector3(-offset, _background.transform.position.y, _background.transform.position.z);
        }
    }
}
