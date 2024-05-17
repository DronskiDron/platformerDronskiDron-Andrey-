﻿using System;
using Creatures.Model.Data;
using Creatures.Model.Definitions;
using UI.Hud.Dialogs;
using UnityEngine;
using UnityEngine.Events;

namespace General.Components.Dialogs
{
    public class ShowDialogComponent : MonoBehaviour
    {
        [SerializeField] private Mode _mode;
        [SerializeField] private DialogData _bound;
        [SerializeField] private DialogDef _external;
        [SerializeField] private UnityEvent _onComplete;
        [SerializeField] private bool _oneSentenceMod = false;

        private DialogBoxController _dialogBox;
        public DialogData Bound => _bound;


        public void Show(int startSentenceValue = 0)
        {
            _dialogBox = FindDialogController();

            if (_dialogBox == null)
                _dialogBox = FindObjectOfType<DialogBoxController>();
            _dialogBox.SetOneSentenceMod(_oneSentenceMod);
            _dialogBox.ShowDialog(Data, _onComplete, startSentenceValue);
        }


        private DialogBoxController FindDialogController()
        {
            if (_dialogBox != null) return _dialogBox;

            GameObject controllerGo;
            switch (Data.Type)
            {
                case DialogType.Simple:
                    controllerGo = GameObject.FindWithTag("SimpleDialog");
                    break;
                case DialogType.Personalized:
                    controllerGo = GameObject.FindWithTag("PersonalizedDialog");
                    break;
                case DialogType.Tutorial:
                    controllerGo = GameObject.FindWithTag("TutorialUI");
                    break;
                default:
                    throw new ArgumentException("Undefined dialog type");
            }

            return controllerGo.GetComponent<DialogBoxController>();
        }


        public void Show(DialogDef def)
        {
            _external = def;
            Show();
        }


        public DialogData Data
        {
            get
            {
                switch (_mode)
                {
                    case Mode.Bound:
                        return _bound;
                    case Mode.External:
                        return _external.Data;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }


        public enum Mode
        {
            Bound,
            External
        }
    }
}
