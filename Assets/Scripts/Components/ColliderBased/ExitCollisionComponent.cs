using UnityEngine;
using Utils;

namespace General.Components.ColliderBased
{
    internal class ExitCollisionComponent : MonoBehaviour
    {
        [SerializeField] private string _tag;
        [SerializeField] private LayerMask _layer = ~0;
        [SerializeField] private EnterEvent _action;


        private void OnCollisionExit2D(Collision2D collision)
        {
            if (!collision.gameObject.IsInLayer(_layer)) return;

            if (!string.IsNullOrEmpty(_tag) && !collision.gameObject.CompareTag(_tag)) return;
            _action?.Invoke(collision.gameObject);
        }
    }
}
