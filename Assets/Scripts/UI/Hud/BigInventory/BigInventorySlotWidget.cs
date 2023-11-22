using System;
using Creatures.Model.Definitions.Repository.Items;
using General.Components.LevelManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Hud.BigInventory
{
    [Serializable]
    public class BigInventorySlotWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private int _slotIndex;
        [SerializeField] private GameObject _selection;
        [InventoryId] public string Id;

        public Image Icon { get; set; }
        public Text TextValue { get; set; }
        public int Value { get; set; }
        public int SlotIndex => _slotIndex;

        private int _childObjArrayIndex = 1;
        private CanvasGroup _canvasGroup;

        public Action Onchanged;


        private void Awake()
        {
            LoadLocalData();
        }


        public void LoadLocalData()
        {
            Icon = transform.GetChild(_childObjArrayIndex).GetComponent<Image>();
            TextValue = Icon.gameObject.transform.GetChild(_childObjArrayIndex).GetComponent<Text>();
        }


        public void ActivateSlot()
        {
            _canvasGroup = Icon.gameObject.GetComponent<CanvasGroup>();
            if (Icon.sprite != null)
                _canvasGroup.alpha = 1;
            TextValue.gameObject.SetActive(true);
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
