﻿using UnityEngine;

namespace Player
{
    public class PlayerJumpChecker : MonoBehaviour
    {
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float _drawSphereRadius = 0.3f;

        private bool _isPressingJump;
        private bool _isTouchingLayer;


        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }


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


        private void OnTriggerStay2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_groundLayer);
        }


        private void OnDrawGizmos()
        {
            Gizmos.color = GetIsGrounded() ? Color.green : Color.red;
            Gizmos.DrawSphere(transform.position, _drawSphereRadius);
        }
    }
}

