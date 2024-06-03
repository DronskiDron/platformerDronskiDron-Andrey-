using Creatures.Model.Data.Properties;
using UnityEngine;

namespace Creatures.Model.Data
{
    [CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private FloatPersistentProperty _music;
        [SerializeField] private FloatPersistentProperty _sfx;
        [SerializeField] private IntPersistentProperty _analytics;
        [SerializeField] private IntPersistentProperty _firstLaunch;
        [SerializeField] private IntPersistentProperty _tutorialenable;

        public FloatPersistentProperty Music => _music;
        public FloatPersistentProperty Sfx => _sfx;
        public IntPersistentProperty Analytics => _analytics;
        public IntPersistentProperty FirstLaunch => _firstLaunch;
        public IntPersistentProperty Tutorialenable => _tutorialenable;

        private static GameSettings _instance;
        public static GameSettings I => _instance == null ? LoadGameSettings() : _instance;


        private static GameSettings LoadGameSettings()
        {
            return _instance = Resources.Load<GameSettings>("GameSettings");
        }


        private void OnEnable()
        {
            _music = new FloatPersistentProperty(1, SoundSetting.Music.ToString());
            _sfx = new FloatPersistentProperty(1, SoundSetting.Sfx.ToString());
            _analytics = new IntPersistentProperty(9, SettingsAmount.AnalyticsStatus.ToString());
            _firstLaunch = new IntPersistentProperty(4, SettingsAmount.FirstLaunch.ToString());
            _tutorialenable = new IntPersistentProperty(0, SettingsAmount.TutorialEnable.ToString());
        }


        private void OnValidate()
        {
            Music.Validate();
            Sfx.Validate();
            Analytics.Validate();
            FirstLaunch.Validate();
            Tutorialenable.Validate();
        }
    }


    public enum SoundSetting
    {
        Music,
        Sfx
    }


    public enum SettingsAmount
    {
        AnalyticsStatus,
        FirstLaunch,
        TutorialEnable
    }
}
