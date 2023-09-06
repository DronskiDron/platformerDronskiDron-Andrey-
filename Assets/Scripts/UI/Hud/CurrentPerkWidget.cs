using Creatures.Model.Data;
using Creatures.Model.Definitions.Repository;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Hud
{
    public class CurrentPerkWidget : MonoBehaviour
    {
        [SerializeField] private Image _icon;
        [SerializeField] private Image _cooldownImage;
        private GameSession _session;


        private void Start()
        {
            _session = FindObjectOfType<GameSession>();
        }


        private void Update()
        {
            var cooldown = _session.PerksModel.Cooldown;
            _cooldownImage.fillAmount = cooldown.RemainingTime / cooldown.Value;
        }


        public void Set(PerkDef perkDef)
        {
            _icon.sprite = perkDef.Icon;
        }
    }
}
