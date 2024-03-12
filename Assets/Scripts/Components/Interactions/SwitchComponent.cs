using Creatures.Model.Data;
using General.Components.LevelManagement;
using UnityEngine;

namespace General.Components.Interactions
{
    public class SwitchComponent : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private bool _state;
        [SerializeField] private string _animationKey;
        [SerializeField] private bool _updateOnStart;
        [SerializeField] private RestoreStateComponent _restoreState;

        private GameSession _session;


        private void Start()
        {
            _session = GameSession.Instance;
            if (_updateOnStart)
                _animator.SetBool(_animationKey, _state);
        }


        public void Switch()
        {
            _state = !_state;
            _animator.SetBool(_animationKey, _state);
            if (_restoreState != null)
                _session?.ItemStateStorage.StoreState(_session.GetCurrentSceneName(), _restoreState.Id);
        }


        [ContextMenu("Switch")]
        public void SwitchIt()
        {
            Switch();
        }
    }
}
