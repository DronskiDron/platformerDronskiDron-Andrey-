using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creatures
{
    public class PlayerJumpCheckerRaycast : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayer;

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
            var hit = Physics2D.Raycast(transform.position, Vector2.down, 1, _groundLayer);
            return hit.collider != null;
        }


        private void OnDrawGizmos()
        {
            Debug.DrawRay(transform.position, Vector3.down, GetIsGrounded() ? Color.green : Color.red);
        }
    }
}

