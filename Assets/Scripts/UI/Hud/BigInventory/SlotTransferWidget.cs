using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace UI.Hud.BigInventory
{
    public class SlotTransferWidget : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        [SerializeField] private BigInventorySlotManager _slotManager;
        [SerializeField] private Image _panel;
        [SerializeField] private BigInventorySlotWidget _parentSlot;
        [SerializeField] private Transform _executiveObj;

        public Image Panel => _panel;
        public BigInventorySlotWidget ParentSlot { get => _parentSlot; set => _parentSlot = value; }
        public CanvasGroup CanvasGroup { get => _canvasGroup; set => _canvasGroup = value; }

        [HideInInspector] public bool WasDone = false;
        private CanvasGroup _canvasGroup;


        private void Start()
        {
            _canvasGroup = gameObject.GetComponent<CanvasGroup>();
        }


        public void OnBeginDrag(PointerEventData eventData)
        {
            transform.SetParent(_executiveObj);
            Panel.gameObject.SetActive(true);
            _canvasGroup.blocksRaycasts = false;
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
                    _slotManager.InconstantItemSprite = _parentSlot.Icon.sprite;
                    _slotManager.InconstantItemValue = _parentSlot.Value.text;

                    _parentSlot.Icon.sprite = donorWidget.ParentSlot.Icon.sprite;
                    _parentSlot.Value.text = donorWidget.ParentSlot.Value.text;
                    _parentSlot.Value.gameObject.SetActive(true);

                    if (_parentSlot.Icon.sprite != null)
                        CanvasGroup.alpha = 1;

                    DonorAfterDrug(donorWidget);

                    donorWidget.ParentSlot.Icon.sprite = _slotManager.InconstantItemSprite;
                    donorWidget.ParentSlot.Value.text = _slotManager.InconstantItemValue;

                    if (donorWidget.ParentSlot.Icon.sprite == null)
                        donorWidget.CanvasGroup.alpha = 0;

                    donorWidget.WasDone = true;
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
    }
}
