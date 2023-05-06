using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace General.Components.ColliderBased
{
    public class CheckLineOverlap : MonoBehaviour
    {
        [SerializeField] private GameObject _target;
        [SerializeField] private LayerMask _mask;
        [SerializeField] private string[] _tags;
        [SerializeField] private OnOverlapEvent _onOverlap;
        [SerializeField] private UnityEvent _selfEvent;

        private readonly RaycastHit2D[] _interactionResult = new RaycastHit2D[10];


        private void OnDrawGizmosSelected()
        {
            Handles.color = HandlesUtils.TranspanentRed;
            Handles.DrawLine(transform.position, _target.transform.position);
        }


        public void Check()
        {
            var size = Physics2D.RaycastNonAlloc(
                transform.position,
                _target.transform.position,
                _interactionResult,
                _mask);

            for (int i = 0; i < size; i++)
            {
                var overlapResult = _interactionResult[i];
                var isInTags = _tags.Any(tag => overlapResult.transform.CompareTag(tag));
                if (isInTags)
                {
                    _selfEvent?.Invoke();
                    _onOverlap?.Invoke(_interactionResult[i].transform.gameObject);
                }
            }
        }
    }
}
