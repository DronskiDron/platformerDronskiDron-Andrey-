﻿using System;
using System.Collections;
using Creatures.Model.Data;
using Creatures.Player;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace UI.Hud.Dialogs
{
    public class DialogBoxController : MonoBehaviour
    {
        [SerializeField] private GameObject _container;
        [SerializeField] private Animator _animator;

        [Space][SerializeField] private float _textSpeed = 0.09f;

        [Header("Sounds")][SerializeField] private AudioClip _typing;
        [SerializeField] private AudioClip _open;
        [SerializeField] private AudioClip _close;

        [Space][SerializeField] protected DialogContent _content;

        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        private UnityEvent _onComplete;
        private UnityEvent _onCompleteSentence;
        private DialogData _data;
        private int _currentSentence;
        private AudioSource _sfxSource;
        private Coroutine _typingRoutine;
        private bool _oneSentenceMod = false;

        protected Sentence CurrentSentence => _data.Sentences[_currentSentence];
        protected Action _sentenceEnded;


        protected virtual void Start()
        {
            _sfxSource = AudioUtils.FindSfxSource();
        }


        public void ShowDialog(DialogData data, UnityEvent onComplete, int startSentenceValue = 0)
        {
            InputEnableComponent.ToggleMenusActivationStatus(false);
            _onComplete = onComplete;
            _data = data;
            _currentSentence = CheckIsSentenceIndexValid(startSentenceValue);
            CurrentContent.Text.text = string.Empty;

            _container.SetActive(true);
            _sfxSource.PlayOneShot(_open);
            _animator.SetBool(IsOpen, true);
        }


        private IEnumerator TypeDialogText()
        {
            CurrentContent.Text.text = string.Empty;
            var sentence = CurrentSentence;
            _onCompleteSentence = CurrentSentence.OnCompleteSentece;
            CurrentContent.TrySetIcon(sentence.Icon, sentence.IconColor);

            foreach (var letter in sentence.Valued)
            {
                CurrentContent.Text.text += letter;
                _sfxSource.PlayOneShot(_typing);
                yield return new WaitForSeconds(_textSpeed);
            }

            _typingRoutine = null;
        }


        protected virtual DialogContent CurrentContent => _content;


        public void OnSkip()
        {
            if (_typingRoutine == null) return;

            StopTypeAnimation();
            CurrentContent.Text.text = _data.Sentences[_currentSentence].Valued;
        }


        public void OnContinue()
        {
            StopTypeAnimation();
            _currentSentence++;
            _onCompleteSentence?.Invoke();

            var isDialogCompleted = _currentSentence >= _data.Sentences.Length;
            if (isDialogCompleted || _oneSentenceMod)
            {
                _currentSentence = CheckIsSentenceIndexValid(_currentSentence);
                HideDialogBox();
                _onComplete?.Invoke();
                _sentenceEnded?.Invoke();
            }
            else
            {
                OnStartDialogAnimation();
            }
        }


        public virtual void OnSkipFullDialog()
        {
            StopTypeAnimation();
            _onCompleteSentence?.Invoke();
            HideDialogBox();
            _onComplete?.Invoke();
            _sentenceEnded?.Invoke();
        }


        private void HideDialogBox()
        {
            _animator.SetBool(IsOpen, false);
            _sfxSource.PlayOneShot(_close);
            InputEnableComponent.ToggleMenusActivationStatus(true);
        }


        private void StopTypeAnimation()
        {
            if (_typingRoutine != null)
                StopCoroutine(_typingRoutine);
            _typingRoutine = null;
        }


        protected virtual void OnStartDialogAnimation()
        {
            _typingRoutine = StartCoroutine(TypeDialogText());
        }


        private void OnCloseAnimationComplete()
        {

        }


        public void SetOneSentenceMod(bool value)
        {
            _oneSentenceMod = value;
        }


        public void SetCurrentSentence(int index)
        {
            _currentSentence = CheckIsSentenceIndexValid(index);
        }


        public void ClearCurrentContent()
        {
            CurrentContent.ClearContent();
        }


        private int CheckIsSentenceIndexValid(int index)
        {
            if (index < _data.Sentences.Length)
                return index;
            else return 0;
        }

    }
}
