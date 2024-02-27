using UnityEngine;
using Utils;

namespace General.Components.ColliderBased
{
    public class LayerCheckCreatures : ColliderCheck
    {
#if UNITY_EDITOR
        [SerializeField] private float _drawSphereRadius = 0.3f;
#endif
        private bool _isPressingJump;

        public LayerMask GroundLayer => Layer;


        public void SetIsPressingJump(Vector2 jumpVector)
        {
            _isPressingJump = jumpVector.y > 0;
        }


        public bool GetIsPressingJump()
        {
            return _isPressingJump;
        }


        public bool GetIsGrounded()
        {
            return _isTouchingLayer;
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = GetIsGrounded() ? HandlesUtils.TranspanentGreen : HandlesUtils.TranspanentRed;
            UnityEditor.Handles.DrawSolidDisc(transform.position, Vector3.forward, _drawSphereRadius);
        }
#endif
    }
}

