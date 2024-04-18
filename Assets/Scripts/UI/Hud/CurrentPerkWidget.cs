using System;
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
        [SerializeField] private Button _button;

        public Button Button => _button;
        public static Action PerkButtonWasPressed;

        private GameSession _session;


        private void Update()
        {
            var cooldown = GameSession.Instance.PerksModel.Cooldown;
            _cooldownImage.fillAmount = cooldown.RemainingTime / cooldown.Value;
        }


        public void Set(PerkDef perkDef)
        {
            _icon.sprite = perkDef.Icon;
        }


        private void OnEnable()
        {
            _button.onClick.AddListener(RunAction);
        }


        private void OnDisable()
        {
            _button.onClick.RemoveListener(RunAction);
        }


        private void RunAction()
        {
            PerkButtonWasPressed?.Invoke();
        }

    }
}
