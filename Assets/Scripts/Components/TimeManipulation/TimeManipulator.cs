using UnityEngine;

namespace General.Components.TimeManipulation
{
    public class TimeManipulator : MonoBehaviour
    {
        public static void StopTime()
        {
            Time.timeScale = 0;
        }


        public static void RunTimeNormal()
        {
            Time.timeScale = 1;
        }


    }
}
