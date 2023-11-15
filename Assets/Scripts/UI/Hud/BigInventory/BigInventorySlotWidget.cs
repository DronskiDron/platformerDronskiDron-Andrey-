using System;
using Creatures.Model.Data;
using Creatures.Model.Definitions.Repository.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Hud.BigInventory
{
    [Serializable]
    public class BigInventorySlotWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        /* public InventorySlotData _data; */
        [SerializeField] private GameObject _selection;
        [InventoryId] public string Id;

        public Image Icon { get => _icon; set => _icon = value; }
        public Text TextValue { get => _textValue; set => _textValue = value; }
        public int Value { get => _value; set => _value = value; }

        private int _childObjArrayIndex = 1;
        private CanvasGroup _canvasGroup;
        private Image _icon;
        private Text _textValue;
        private int _value;


        private void Awake()
        {
            LoadLocalData();
        }


        public void LoadLocalData()
        {
            _icon = transform.GetChild(_childObjArrayIndex).GetComponent<Image>();
            _textValue = _icon.gameObject.transform.GetChild(_childObjArrayIndex).GetComponent<Text>();
        }


        public void ActivateSlot()
        {
            _canvasGroup = _icon.gameObject.GetComponent<CanvasGroup>();
            if (_icon.sprite != null)
                _canvasGroup.alpha = 1;
            _textValue.gameObject.SetActive(true);
        }


        public void OnPointerEnter(PointerEventData eventData)
        {
            _selection.gameObject.SetActive(true);
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            _selection.gameObject.SetActive(false);
        }


        private void OnDisable()
        {
            _selection.gameObject.SetActive(false);
        }
    }
}
