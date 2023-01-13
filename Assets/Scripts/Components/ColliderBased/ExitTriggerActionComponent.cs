using UnityEngine;

namespace General.Components.ColliderBased
{
    public class ExitTriggerActionComponent : MonoBehaviour
    {
        [SerializeField] private ColliderCheck _layerCheck;
        [SerializeField] private EnterEvent _action;


        private void OnExitTriggerAction(Collision2D collision)
        {
            if (_layerCheck.IsTouchingLayer == false)
            {
                _action?.Invoke(collision.gameObject);
            }
        }
    }
}
