using Creatures.Model.Definitions.Localisation;
using General.Components.Dialogs;
using UI.Hud.Dialogs;
using UnityEngine;

namespace UI.Localization
{
    public class LocalizeOptionDialogText : AbstractLocalizeComponent
    {
        [SerializeField] private string[] _keys;
        [SerializeField] private bool _capitalize;

        private ShowOptionsComponent _showOption;
        private OptionData[] _options;


        protected override void Awake()
        {
            _showOption = GetComponent<ShowOptionsComponent>();
            _options = _showOption.Data.Options;
            base.Awake();
        }


        protected override void Localize()
        {
            for (int k = 0; k < _keys.Length; k++)
            {
                if (k == 0)
                {
                    _showOption.Data.DialogText = LocalizationManager.I.Localize(_keys[k]);
                    _showOption.Data.DialogText = _capitalize ? _showOption.Data.DialogText.ToUpper() : _showOption.Data.DialogText;
                }
                for (int i = 0; i < _options.Length; i++)
                {
                    if (i == k - 1)
                    {
                        var localized = LocalizationManager.I.Localize(_keys[k]);
                        _options[i].Text = _capitalize ? localized.ToUpper() : localized;
                    }
                }
            }
        }
    }
}
