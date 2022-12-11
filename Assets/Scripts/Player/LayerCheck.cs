using UnityEditor;
using UnityEngine;
using Utils;

namespace Creatures
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] private LayerMask _layer;
        [SerializeField] private bool _isTouchingLayer;
        [SerializeField] private Collider2D _collider;
        [SerializeField] private float _drawSphereRadius = 0.3f;

        private bool _isPressingJump;
        public bool IsTouchingLayer => _isTouchingLayer;

        public LayerMask GroundLayer => _layer;


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
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(_layer);
        }


#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            Handles.color = GetIsGrounded() ? HandlesUtils.TranspanentGreen : HandlesUtils.TranspanentRed;
            Handles.DrawSolidDisc(transform.position, Vector3.forward, _drawSphereRadius);
        }
#endif
    }
}

