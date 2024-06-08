using System;
using UnityEngine;

namespace PirateIsland.Plot
{
    public class CutsceneController : MonoBehaviour
    {
        public Action OnShowCutscene;


        public void OnCutsceneTriggered()
        {
            OnShowCutscene?.Invoke();
        }
    }
}
