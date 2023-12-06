using Creatures.Model.Definitions;
using Creatures.Model.Definitions.Items;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Hud.BigInventory
{
    public class SlotTransferWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
    {
        [SerializeField] private BigInventoryTemp _temp;
        [SerializeField] private Image _panel;
        [SerializeField] private BigInventorySlotWidget _parentSlot;
        [SerializeField] private Transform _executiveObj;

        public Image Panel => _panel;
        public BigInventorySlotWidget ParentSlot { get => _parentSlot; private set => _parentSlot = value; }
        public CanvasGroup CanvasGroup { get; set; }

        [HideInInspector] public bool WasDone = false;
        private ItemInfoController _infoController;


        private void Start()
        {
            CanvasGroup = gameObject.GetComponent<CanvasGroup>();
            _infoController = FindObjectOfType<ItemInfoController>();
        }


        public void OnPointerClick(PointerEventData eventData)
        {
            _infoController?.RenewItemInfo(_parentSlot);
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            _infoController?.RenewItemInfo(_parentSlot);
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
                    _temp.RenewTemp(_parentSlot);
                    ExchangeInfo(_parentSlot, donorWidget.ParentSlot);
                    _parentSlot.TextValue.gameObject.SetActive(true);

                    if (_parentSlot.Icon.sprite != null)
                        CanvasGroup.alpha = 1;

                    DonorAfterDrug(donorWidget);
                    _temp.RenewFromTemp(donorWidget.ParentSlot);

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


        private void ExchangeInfo(BigInventorySlotWidget acceptorWidget, BigInventorySlotWidget donorWidget)
        {
            acceptorWidget.Id = donorWidget.Id;
            acceptorWidget.Icon.sprite = donorWidget.Icon.sprite;
            acceptorWidget.TextValue.text = donorWidget.TextValue.text;
            acceptorWidget.Value = donorWidget.Value;
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
