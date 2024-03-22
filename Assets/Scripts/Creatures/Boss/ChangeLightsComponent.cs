using UnityEngine;


namespace Creatures.Boss
{
    public class ChangeLightsComponent : MonoBehaviour
    {
        [SerializeField] private UnityEngine.Rendering.Universal.Light2D[] _lights;

        [ColorUsage(true, true)]
        [SerializeField] private Color _color;


        [ContextMenu("Setup")]
        public void SetColor()
        {
            foreach (var light2D in _lights)
            {
                light2D.color = _color;
            }
        }

    }
}
