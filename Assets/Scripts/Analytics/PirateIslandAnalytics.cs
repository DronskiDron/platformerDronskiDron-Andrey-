using Unity.Services.Analytics;
using UnityEngine;

namespace PirateIsland.Analytics
{
    public class PirateIslandAnalytics : MonoBehaviour
    {
        public static void TrackThatLevelWasStarted(int levelIndex)
        {
            if (!InitAnalyticsComponent.IsAnalyticsAllow) return;

            var myEvent = new CustomEvent("LevelWasStarted")
{
    { "NumericalValue", levelIndex }
};
            AnalyticsService.Instance.RecordEvent(myEvent);
            Debug.Log("EventName " + levelIndex + myEvent);
        }


        public static void TrackThatLevelWasCompleted(int levelIndex)
        {
            if (!InitAnalyticsComponent.IsAnalyticsAllow) return;

            var myEvent = new CustomEvent("LevelWasCompleted")
{
    { "NumericalValue", levelIndex }
};
            AnalyticsService.Instance.RecordEvent(myEvent);
            Debug.Log("EventName " + levelIndex + myEvent);
        }
    }
}
