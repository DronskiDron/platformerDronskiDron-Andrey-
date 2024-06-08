using UnityEngine;
using UnityEngine.Events;

namespace PirateIsland.Plot
{
    public class CutsceneItemComponent : MonoBehaviour
    {
        [SerializeField] private CutsceneController _controller;
        [SerializeField] private UnityEvent _onActiavate;


        private void Start()
        {
            _controller.OnShowCutscene += ActivateEvent;
        }


        private void OnDestroy()
        {
            _controller.OnShowCutscene -= ActivateEvent;
        }


        private void ActivateEvent()
        {
            _onActiavate?.Invoke();
        }
    }
}
