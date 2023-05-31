using Creatures.Model.Data;
using Creatures.Model.Definitions.Localisation;
using General.Components.Dialogs;
using UnityEngine;

namespace UI.Localization
{
    public class LocalizeDialogText : AbstractLocalizeComponent
    {
        [SerializeField] private string[] _keys;
        [SerializeField] private bool _capitalize;

        private ShowDialogComponent _dialogComponent;
        private Sentence[] _sentences;


        protected override void Awake()
        {
            _dialogComponent = GetComponent<ShowDialogComponent>();
            _sentences = _dialogComponent.Bound.Sentences;
            base.Awake();
        }


        protected override void Localize()
        {
            for (int k = 0; k < _sentences.Length; k++)
            {
                for (int i = 0; i < _keys.Length; i++)
                {
                    if (k == i)
                    {
                        var localized = LocalizationManager.I.Localize(_keys[i]);
                        _sentences[k].Valued = _capitalize ? localized.ToUpper() : localized;
                    }
                }
            }
        }
    }
}
