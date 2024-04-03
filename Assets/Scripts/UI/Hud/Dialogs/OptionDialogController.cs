﻿using System;
using Creatures.Player;
using UI.Widgets;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI.Hud.Dialogs
{
    public class OptionDialogController : MonoBehaviour
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private Text _contentText;
        [SerializeField] private Transform _optionsContainer;
        [SerializeField] private OptionItemWidget _prefab;
        [SerializeField] private InputEnableComponent _inputEnabler;
        [SerializeField] private bool _blockInput = false;

        private DataGroup<OptionData, OptionItemWidget> _dataGroup;


        private void Start()
        {
            _dataGroup = new DataGroup<OptionData, OptionItemWidget>(_prefab, _optionsContainer);
        }


        public void OnOptionsSelected(OptionData selectedOPption)
        {
            selectedOPption.OnSelect.Invoke();
            _content.SetActive(false);
        }


        public void Show(OptionDialogData data)
        {
            _content.SetActive(true);
            _contentText.text = data.DialogText;
            _dataGroup.SetData(data.Options);
            if (_blockInput)
                _inputEnabler.SetInputDisabled();
        }
    }


    [Serializable]
    public class OptionDialogData
    {
        public string DialogText;
        public OptionData[] Options;
    }


    [Serializable]
    public class OptionData
    {
        public string Text;
        public UnityEvent OnSelect;
    }
}
