using System.Collections;
using UnityEngine;

namespace General.Components.Perks
{
    public class PowerWaveController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        private RectTransform _rectTransform;


        private void Start()
        {
            _rectTransform = _spriteRenderer.gameObject.GetComponent<RectTransform>();
            ChangeScale(0f);
        }


        private void ChangeScale(float value)
        {
            var scale = _rectTransform.localScale;
            _rectTransform.localScale = new Vector3(value, value, value);
        }


        public void StartPowerWaveAnimation()
        {
            StartCoroutine(PowerWaveShow());
        }


        private IEnumerator PowerWaveShow()
        {
            ChangeScale(1f);
            yield return new WaitForSeconds(0.1f);
            ChangeScale(2f);
            yield return new WaitForSeconds(0.1f);
            ChangeScale(3f);
            yield return new WaitForSeconds(0.1f);
            ChangeScale(4f);
            yield return new WaitForSeconds(0.1f);
            ChangeScale(0f);
        }
    }
}
