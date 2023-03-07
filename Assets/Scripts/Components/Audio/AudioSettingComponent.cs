using System;
using Creatures.Model.Data;
using Creatures.Model.Data.Properties;
using UnityEngine;
using Utils;

namespace General.Components.Audio
{
    public class AudioSettingComponent : MonoBehaviour
    {
        [SerializeField] private SoundSetting _mode;

        private FloatPersistentProperty _model;
        private AudioSource _source;


        private void Start()
        {
            _source = GetComponent<AudioSource>() != null ? GetComponent<AudioSource>() : AudioUtils.FindSfxSource();

            _model = FindProperty();
            _model.OnChanged += OnSoundSettingChanged;
            OnSoundSettingChanged(_model.Value, _model.Value);
        }


        private void OnSoundSettingChanged(float newValue, float oldValue)
        {
            _source.volume = newValue;
        }


        private FloatPersistentProperty FindProperty()
        {
            switch (_mode)
            {
                case SoundSetting.Music:
                    return GameSettings.I.Music;
                case SoundSetting.Sfx:
                    return GameSettings.I.Sfx;
            }

            throw new ArgumentException("Undefined mode");
        }


        private void OnDestroy()
        {
            _model.OnChanged -= OnSoundSettingChanged;
        }
    }
}
