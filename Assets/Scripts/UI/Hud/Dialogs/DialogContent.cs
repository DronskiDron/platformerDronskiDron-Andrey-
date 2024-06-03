using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud.Dialogs
{
    public class DialogContent : MonoBehaviour
    {
        [SerializeField] private Text _text;
        [SerializeField] private Image _icon;

        public Text Text => _text;


        public void TrySetIcon(Sprite icon, Color iconColor)
        {
            _icon?.gameObject.SetActive(false);

            if (icon != null)
            {
                _icon.sprite = icon;
                SetIconColor(iconColor);
                _icon.gameObject.SetActive(true);
            }
        }


        public void ClearContent()
        {
            _text.text = "";
            _icon?.gameObject.SetActive(false);
        }


        private void SetIconColor(Color iconColor)
        {
            _icon.color = iconColor;
        }
    }
}
