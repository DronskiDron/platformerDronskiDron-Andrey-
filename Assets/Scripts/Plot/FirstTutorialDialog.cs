using Creatures.Model.Data;
using General.Components.Interactions;
using UnityEngine;
using UnityEngine.Events;

namespace PirateIsland.Plot
{
    public class FirstTutorialDialog : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onStart;

        private InteractableComponent _interaction;


        private void Start()
        {
            if (GameSession.Instance.TutorialStarted) return;

            _interaction = GetComponent<InteractableComponent>();

            GameSession.Instance.SetTutorialStatusFlag(true);
            Invoke("RunStartEvent", 0.2f);
            Invoke("Interact", 1f);
        }


        public void Interact()
        {
            _interaction.Interact();
        }


        private void RunStartEvent()
        {
            _onStart?.Invoke();
        }
    }
}
