using UnityEngine;

namespace UI.Windows.MainMenu
{
    public class AnimatedUIComponent : MonoBehaviour
    {
        [SerializeField] private RectTransform _canvasRectTransform;
        [SerializeField] private RectTransform _selfRectTransform;
        [SerializeField] private RectTransform _childRectTransform;
        [SerializeField] private float _moveSpeed = 5f;

        private Vector2 _initialPosition;
        private float _moveDistance;
        private float _currentDistanceMoved = 0f;


        private void Start()
        {
            SetOffsetX(_childRectTransform, _canvasRectTransform);

            _moveDistance = _canvasRectTransform.sizeDelta.x;
            _initialPosition = _selfRectTransform.anchoredPosition;
        }


        private void Update()
        {
            SetOffsetX(_childRectTransform, _canvasRectTransform);
            _moveDistance = _canvasRectTransform.sizeDelta.x;

            var distanceThisFrame = _moveSpeed * Time.deltaTime;
            _currentDistanceMoved += distanceThisFrame;

            if (_currentDistanceMoved < _moveDistance)
            {
                _selfRectTransform.anchoredPosition -= new Vector2(distanceThisFrame, 0);
            }
            else
            {
                _selfRectTransform.anchoredPosition = _initialPosition;
                _currentDistanceMoved = 0f;
            }
        }


        private void SetOffsetX(RectTransform objNeedChange, RectTransform reference)
        {
            var offsetMin = objNeedChange.offsetMin;
            var offsetMax = objNeedChange.offsetMax;

            offsetMin.x = reference.sizeDelta.x;
            offsetMax.x = reference.sizeDelta.x;

            objNeedChange.offsetMin = offsetMin;
            objNeedChange.offsetMax = offsetMax;
        }
    }
}
