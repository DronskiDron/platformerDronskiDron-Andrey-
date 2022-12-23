using UnityEngine;

namespace General.Components.Interactions
{
    public class DoInteractionComponent : MonoBehaviour
    {
        public void DoInteraction(GameObject go)
        {
            var interactable = go.GetComponent<InteractableComponent>();
            interactable?.Interact();
        }
    }
}
