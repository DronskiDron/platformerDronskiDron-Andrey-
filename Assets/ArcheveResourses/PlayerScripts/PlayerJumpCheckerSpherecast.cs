using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures
{
    public class PlayerJumpCheckerSpherecast : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private Vector3 _groundCheckPositionDelta;

        private bool _isJumping;


        public void SetIsJumping(Vector2 jumpVector)
        {
            _isJumping = jumpVector.y > 0;
        }


        public bool GetIsJumping()
        {
            return _isJumping;
        }


        public bool GetIsGrounded()
        {
            var hit = Physics2D.CircleCast(transform.position + _groundCheckPositionDelta, _groundCheckRadius, Vector3.down, 0, _groundLayer);
            return hit.collider != null;
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = GetIsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position + _groundCheckPositionDelta, _groundCheckRadius);
        }
    }
}

