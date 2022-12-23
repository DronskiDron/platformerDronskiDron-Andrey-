using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace General.Components.ColliderBased
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] private float _radius = 1;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;

        private readonly Collider2D[] _interactionResult = new Collider2D[10];


        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TranspanentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _radius);
        }


        public void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                _radius,
                _interactionResult,
                _mask);

            for (int i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i];
                var isInTags = _tags.Any(tag => overlapResult.CompareTag(tag));
                if (isInTags)
                {
                    _onOverlap?.Invoke(_interactionResult[i].gameObject);
                }
            }
        }


        [Serializable]
        public class OnOverlapEvent : UnityEvent<GameObject>
        {
        }
    }
}
