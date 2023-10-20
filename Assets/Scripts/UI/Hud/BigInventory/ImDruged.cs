using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace UI.Hud.BigInventory
{
    public class ImDruged : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
    {
        /* [SerializeField] private SlotTransferWidget _transfer; */

        public void OnBeginDrag(PointerEventData eventData)
        {
            /* Debug.Log("I am begin"); */
        }

        public void OnDrag(PointerEventData eventData)
        {
            /* Debug.Log("I am in process"); */
            /*  gameObject.transform.position = Mouse.current.position.ReadValue(); */
        }

        public void OnDrop(PointerEventData eventData)
        {
            Debug.Log("Vot ato povorot!!!");
        }

        public void OnEndDrag(PointerEventData eventData)
        {

        }
    }
}

