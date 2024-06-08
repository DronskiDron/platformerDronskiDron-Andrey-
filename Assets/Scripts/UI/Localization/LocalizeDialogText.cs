using System;
using Creatures.Model.Data;
using Creatures.Model.Definitions.Localisation;
using General.Components.Dialogs;
using UnityEngine;

namespace UI.Localization
{
    public class LocalizeDialogText : AbstractLocalizeComponent
    {
        [SerializeField] private DialogKeys[] _dialogKeys;
        [SerializeField] private bool _capitalize;

        private ShowDialogComponent _dialogComponent;
        private Sentence[] _sentences;


        protected override void Awake()
        {
            _dialogComponent = GetComponent<ShowDialogComponent>();
            _sentences = _dialogComponent.Data.Sentences;

            base.Awake();
        }


        protected override void Localize()
        {
            for (int k = 0; k < _sentences.Length; k++)
            {
                for (int i = 0; i < _dialogKeys.Length; i++)
                {
                    if (k == i)
                    {
                        var key = !Application.isMobilePlatform ? _dialogKeys[i].Key : _dialogKeys[i].MobileKey;
                        var localized = LocalizationManager.I.Localize(key);
                        _sentences[k].Valued = _capitalize ? localized.ToUpper() : localized;
                    }
                }
            }
        }
    }


    [Serializable]
    public class DialogKeys
    {
        [SerializeField] private int _index;
        [SerializeField] private string _key;
        [SerializeField] private string _mobileKey;

        public string Key => _key;
        public string MobileKey => _mobileKey;
    }
}
