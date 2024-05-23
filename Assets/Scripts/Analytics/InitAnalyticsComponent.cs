using Creatures.Model.Data;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

namespace PirateIsland.Analytics
{
    public class InitAnalyticsComponent : MonoBehaviour
    {
        private static bool _isAnalyticsAllow = false;
        private bool _dataCollectionWasStarted = false;

        public static bool IsAnalyticsAllow => _isAnalyticsAllow;


        async void InitServices()
        {
            await UnityServices.InitializeAsync();
            Debug.Log("Unity services were inited!");
        }


        public void InitUnityServices()
        {
            InitServices();
            _isAnalyticsAllow = GameSettings.I.Analytics.Value == 1 ? true : false;
        }


        public void BeginDataCollection()
        {
            if (GameSettings.I.Analytics.Value == 1)
            {
                AnalyticsService.Instance.StartDataCollection();
                _dataCollectionWasStarted = true;
                Debug.Log("Data collection began!");
            }
        }


        public void StopDataCollection()
        {
            Debug.Log("Data collection was stoped!");

            if (!_dataCollectionWasStarted) return;

            AnalyticsService.Instance.StopDataCollection();
        }


        public void SetAnalyticsStatus(bool value)
        {
            GameSettings.I.Analytics.Value = value ? 1 : 0;

            if (value)
                BeginDataCollection();
            else
                StopDataCollection();

            _isAnalyticsAllow = value;
        }
    }
}
