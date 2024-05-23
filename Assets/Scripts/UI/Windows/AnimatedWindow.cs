using System;
using UnityEngine;
using UnityEngine.Events;

namespace UI.Windows
{
    public class AnimatedWindow : MonoBehaviour
    {
        public UnityEvent _onStartEvent;
        public UnityEvent _onCloseEvent;

        public static Action OnWindowAppeared;

        private Animator _animator;

        private static readonly int Show = Animator.StringToHash("Show");
        private static readonly int Hide = Animator.StringToHash("Hide");


        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
            _animator.SetTrigger(Show);
            OnWindowAppeared?.Invoke();
            _onStartEvent?.Invoke();
        }


        public void Close()
        {
            _animator.SetTrigger(Hide);
            _onCloseEvent?.Invoke();
        }


        public virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }
    }
}
