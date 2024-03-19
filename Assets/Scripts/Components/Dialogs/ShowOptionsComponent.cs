using UI.Hud.Dialogs;
using UnityEngine;

namespace General.Components.Dialogs
{
    public class ShowOptionsComponent : MonoBehaviour
    {
        [SerializeField] private OptionDialogData _data;

        public OptionDialogData Data { get => _data; set => _data = value; }

        private OptionDialogController _dialogBox;


        public static bool _letNextPhrase = true;


        public void Show()
        {
            if (_dialogBox == null)
                _dialogBox = FindObjectOfType<OptionDialogController>();
            _dialogBox.Show(Data);
        }
    }
}
