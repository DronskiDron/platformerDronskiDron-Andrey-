using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.Components.TimeManipulation
{
    public class StopTimeComponent : MonoBehaviour
    {
        public void StopTime()
        {
            Time.timeScale = 0;
        }


        public void RunTimeNormal()
        {
            Time.timeScale = 1;
        }
    }
}

