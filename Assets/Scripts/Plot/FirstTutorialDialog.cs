using General.Components.Interactions;
using UnityEngine;

namespace PirateIsland.Plot
{
    public class FirstTutorialDialog : MonoBehaviour
    {
        private InteractableComponent _interaction;

        private void Start()
        {
            _interaction = GetComponent<InteractableComponent>();

            Invoke("Interact", 1f);
        }

        public void Interact()
        {
            _interaction.Interact();
        }
    }
}
