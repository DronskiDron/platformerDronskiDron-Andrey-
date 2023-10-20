using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Hud.BigInventory
{
    public class BigInventorySlotWidget : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private GameObject _selection;

        public Image Icon { get => _icon; set => _icon = value; }
        public Text Value { get => _value; set => _value = value; }

        private int _childObjArrayIndex = 1;
        private CanvasGroup _canvasGroup;
        private Image _icon;
        private Text _value;


        private void Awake()
        {
            LoadLocalData();
        }


        public void LoadLocalData()
        {
            _icon = transform.GetChild(_childObjArrayIndex).GetComponent<Image>();
            _value = _icon.gameObject.transform.GetChild(_childObjArrayIndex).GetComponent<Text>();
        }


        public void ActivateSlot()
        {
            _canvasGroup = _icon.gameObject.GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 1;
            _value.gameObject.SetActive(true);
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
