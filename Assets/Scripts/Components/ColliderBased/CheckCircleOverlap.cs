﻿using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace General.Components.ColliderBased
{
    public class CheckCircleOverlap : MonoBehaviour
    {
        [SerializeField] protected float Radius = 1;
        [SerializeField] protected LayerMask Mask;
        [SerializeField] protected string[] Tags;
        [SerializeField] protected OnOverlapEvent OnOverlap;

        protected readonly Collider2D[] InteractionResult = new Collider2D[10];


#if UNITY_EDITOR
        private void OnDrawGizmosSelected()
        {
            UnityEditor.Handles.color = HandlesUtils.TranspanentRed;
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, Radius);
        }
#endif


        public void Check()
        {
            var size = Physics2D.OverlapCircleNonAlloc(
                transform.position,
                Radius,
                InteractionResult,
                Mask);

            for (int i = 0; i < size; i++)
            {
                var overlapResult = InteractionResult[i];
                var isInTags = Tags.Any(tag => overlapResult.CompareTag(tag));
                if (isInTags)
                {
                    OnOverlap?.Invoke(InteractionResult[i].gameObject);
                }
            }
        }
    }


    [Serializable]
    public class OnOverlapEvent : UnityEvent<GameObject>
    {
    }
}
