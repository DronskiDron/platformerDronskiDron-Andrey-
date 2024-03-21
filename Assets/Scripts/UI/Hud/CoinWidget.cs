using Creatures.Model.Data;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class CoinWidget : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private Text _text;

        private GameSession _session;


        private void Start()
        {
            _session = GameSession.Instance;
            _session.Data.Inventory.OnChanged += OnTextChanged;
            OnTextChanged(default, default);
        }


        private void OnTextChanged(string id, int value)
        {
            var coinCount = _session.Data.Inventory.GetItem("Coin")?.Value;

            if (coinCount == null)
                coinCount = 0;

            _text.text = coinCount.ToString();
        }


        private void OnDestroy()
        {
            _session.Data.Inventory.OnChanged -= OnTextChanged;
        }
    }
}
