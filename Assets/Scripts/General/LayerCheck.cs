using UnityEngine;

namespace General.Components
{
    public class LayerCheck : MonoBehaviour
    {
        [SerializeField] protected LayerMask Layer;
        [SerializeField] protected bool _isTouchingLayer;
        [SerializeField] private Collider2D _collider;

        public bool IsTouchingLayer => _isTouchingLayer;


        private void Awake()
        {
            _collider = GetComponent<Collider2D>();
        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(Layer);
        }


        private void OnTriggerExit2D(Collider2D collision)
        {
            _isTouchingLayer = _collider.IsTouchingLayers(Layer);
        }
    }
}
