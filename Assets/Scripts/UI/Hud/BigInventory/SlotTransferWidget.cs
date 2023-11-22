using Creatures.Model.Data;
using Creatures.Model.Definitions;
using Creatures.Model.Definitions.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Hud.BigInventory
{
    public class SlotTransferWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private BigInventoryTemp _temp;
        [SerializeField] private Image _panel;
        [SerializeField] private BigInventorySlotWidget _parentSlot;
        [SerializeField] private Transform _executiveObj;

        public Image Panel => _panel;
        public BigInventorySlotWidget ParentSlot { get => _parentSlot; private set => _parentSlot = value; }
        public CanvasGroup CanvasGroup { get; set; }

        [HideInInspector] public bool WasDone = false;


        private void Start()
        {
            CanvasGroup = gameObject.GetComponent<CanvasGroup>();
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            _temp.SlotIndex = _parentSlot.SlotIndex;
            transform.SetParent(_executiveObj);
            Panel.gameObject.SetActive(true);
            CanvasGroup.blocksRaycasts = false;
            WasDone = false;
        }


        public void OnDrag(PointerEventData eventData)
        {
            gameObject.transform.position = Mouse.current.position.ReadValue();
        }


        public void OnEndDrag(PointerEventData eventData)
        {
            if (!WasDone)
            {
                AcceptorAfterDrug();
            }
        }


        public void OnDrop(PointerEventData eventData)
        {
            var draggedObject = eventData.pointerDrag;
            if (draggedObject != null)
            {
                var donorWidget = draggedObject.GetComponent<SlotTransferWidget>();
                if (donorWidget.ParentSlot.Icon.sprite != null)
                {
                    _temp.InconstantId = _parentSlot.Id;
                    _temp.InconstantItemSprite = _parentSlot.Icon.sprite;
                    _temp.InconstantItemTextValue = _parentSlot.TextValue.text;
                    _temp.InconstantValue = _parentSlot.Value;

                    _parentSlot.Id = donorWidget.ParentSlot.Id;
                    _parentSlot.Icon.sprite = donorWidget.ParentSlot.Icon.sprite;
                    _parentSlot.TextValue.text = donorWidget.ParentSlot.TextValue.text;
                    _parentSlot.Value = donorWidget.ParentSlot.Value;
                    _parentSlot.TextValue.gameObject.SetActive(true);

                    if (_parentSlot.Icon.sprite != null)
                        CanvasGroup.alpha = 1;

                    DonorAfterDrug(donorWidget);

                    donorWidget.ParentSlot.Id = _temp.InconstantId;
                    donorWidget.ParentSlot.Icon.sprite = _temp.InconstantItemSprite;
                    donorWidget.ParentSlot.TextValue.text = _temp.InconstantItemTextValue;
                    donorWidget.ParentSlot.Value = _temp.InconstantValue;

                    if (donorWidget.ParentSlot.Icon.sprite == null)
                        donorWidget.CanvasGroup.alpha = 0;

                    donorWidget.WasDone = true;

                    _parentSlot.Onchanged?.Invoke();
                }
            }

        }


        private void AcceptorAfterDrug()
        {
            var nativeObj = ParentSlot.gameObject.transform;
            transform.SetParent(nativeObj);
            _parentSlot.LoadLocalData();
            gameObject.transform.position = nativeObj.position;
            Panel.gameObject.SetActive(false);
            gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }


        public void DonorAfterDrug(SlotTransferWidget donorWidget)
        {
            var nativeObj = donorWidget.ParentSlot.gameObject.transform;
            donorWidget.gameObject.transform.SetParent(nativeObj);
            donorWidget.ParentSlot.LoadLocalData();
            donorWidget.gameObject.SetActive(false);
            donorWidget.gameObject.transform.position = nativeObj.position;
            donorWidget.Panel.gameObject.SetActive(false);
            donorWidget.gameObject.SetActive(true);
            donorWidget.gameObject.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }


        public void ClearSlot()
        {
            var def = DefsFacade.I.Items.Get(_parentSlot.Id);
            if (def.HasTag(ItemTag.OneMustStay))
            {
                _parentSlot.Value = 1;
                _parentSlot.TextValue.text = _parentSlot.Value.ToString();
            }
            else
            {
                _parentSlot.Id = "None";
                _parentSlot.Icon.sprite = null;
                _parentSlot.TextValue.text = null;
                _parentSlot.Value = 0;
                CanvasGroup.alpha = 0;
            }
        }
    }
}
