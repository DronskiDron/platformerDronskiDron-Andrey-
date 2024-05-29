﻿using Creatures.Model.Data;
using UnityEngine;

namespace UI.Hud.Dialogs
{
    public class PersonalizedDialogBoxController : DialogBoxController
    {
        [SerializeField] private DialogContent _right;

        protected override DialogContent CurrentContent => CurrentSentence.Side == Side.Left ? _content : _right;


        protected override void Start()
        {
            base.Start();
            _sentenceEnded += DeactivateCurrentSide;
        }


        protected override void OnStartDialogAnimation()
        {
            _right.gameObject.SetActive(CurrentSentence.Side == Side.Right);
            _content.gameObject.SetActive(CurrentSentence.Side == Side.Left);

            base.OnStartDialogAnimation();
        }


        private void DeactivateCurrentSide()
        {
            CurrentContent.gameObject.SetActive(false);
        }


        private void OnDestroy()
        {
            _sentenceEnded -= DeactivateCurrentSide;
        }

    }
}
