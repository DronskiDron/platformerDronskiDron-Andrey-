using UnityEngine;
using UnityEngine.Events;

namespace General.Components.Interactions
{
    public class InteractableComponent : MonoBehaviour
    {
        [SerializeField] private UnityEvent _action;


        public void Interact()
        {
            _action?.Invoke();
        }
    }
}

